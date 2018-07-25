//2015-2016, MIT, EngineKit 
/* The MIT License
*
* Copyright (c) 2012-2015 sta.blockhead
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{

    class WebSocketProtocolParser
    {

        enum ParseState
        {
            Init,
            ReadExtendedPayloadLen,
            ReadMask,
            ExpectBody,
            Complete
        }

        Queue<WebSocketRequest> incommingReqs = new Queue<WebSocketRequest>();
        RecvIOBufferStream myBufferStream;
        WebSocketRequest currentReq;
        RecvIO recvIO;
        ParseState parseState;

        int _currentPacketLen;
        int _currentMaskLen;
        //-----------------------
        byte[] maskKey = new byte[4];
        byte[] fullPayloadLengthBuffer = new byte[8];
        bool useMask;
        Opcode currentOpCode = Opcode.Cont;//use default 
        //-----------------------
        WebSocketContext _ownerContext;
        internal WebSocketProtocolParser(WebSocketContext context, RecvIO recvIO)
        {
            this.recvIO = recvIO;
            this._ownerContext = context;
            myBufferStream = new RecvIOBufferStream(recvIO);
        }
        public int ReqCount
        {
            get
            {
                return incommingReqs.Count;
            }
        }
        public WebSocketRequest Dequeue()
        {
            return incommingReqs.Dequeue();
        }

        bool ReadHeader()
        {
            if (!myBufferStream.Ensure(2))
            {
                myBufferStream.BackupRecvIO();
                return false;
            }
            //----------------------------------------------------------
            //when we read header we start a new websocket request
            currentReq = new WebSocketRequest(this._ownerContext);
            incommingReqs.Enqueue(currentReq);


            byte b1 = myBufferStream.ReadByte();
            // FIN
            Fin fin = (b1 & (1 << 7)) == (1 << 7) ? Fin.Final : Fin.More;

            // RSV1
            Rsv rsv1 = (b1 & (1 << 6)) == (1 << 6) ? Rsv.On : Rsv.Off;

            // RSV2
            Rsv rsv2 = (b1 & (1 << 5)) == (1 << 5) ? Rsv.On : Rsv.Off;

            // RSV3
            Rsv rsv3 = (b1 & (1 << 4)) == (1 << 4) ? Rsv.On : Rsv.Off;
            //----------------------------------------------------------   
            // Opcode
            currentOpCode = (Opcode)(b1 & 0x0f);//4 bits  
            //----------------------------------------------------------  
            byte b2 = myBufferStream.ReadByte();          //mask

            //----------------------------------------------------------  
            //finish first 2 bytes  
            // MASK
            Mask currentMask = (b2 & (1 << 7)) == (1 << 7) ? Mask.On : Mask.Off;
            //we should check receive frame here ... 
            this.useMask = currentMask == Mask.On;
            if (currentMask == Mask.Off)
            {
                //if this act as WebSocketServer 
                //erro packet ? 
                throw new NotSupportedException();
            }
            else
            {

            }
            //----------------------------------------------------------
            // Payload Length
            byte payloadLen = (byte)(b2 & 0x7f); //is 7 bits of the b2 
            if (fin == Fin.More || currentOpCode == Opcode.Cont)
            {
                //process fragment frame *** 
                throw new NotSupportedException();
            }
            else
            {

            }

            //----------------------------------------------------------
            //translate opcode ....
            string errCode = null;
            switch (currentOpCode)
            {
                case Opcode.Cont:
                    {
                        //continue
                    }
                    break;
                case Opcode.Text: //this is data
                    {
                        if (rsv1 == Rsv.On)
                        {
                            errCode = "A non data frame is compressed.";
                        }
                    }
                    break;
                case Opcode.Binary: //this is data
                    {
                        if (rsv1 == Rsv.On)
                        {
                            errCode = "A non data frame is compressed.";
                        }
                    }
                    break;
                case Opcode.Close: //control
                    {
                        if (fin == Fin.More)
                        {
                            errCode = "A control frame is fragmented.";
                        }
                        else if (payloadLen > 125)
                        {
                            errCode = "A control frame has a long payload length.";
                        }
                    }
                    break;
                case Opcode.Ping: //control
                case Opcode.Pong: //control
                    {
                        if (fin == Fin.More)
                        {
                            errCode = "A control frame is fragmented.";
                        }
                        else if (payloadLen > 125)
                        {
                            errCode = "A control frame has a long payload length.";
                        }
                    }
                    break;
                default:
                    {
                        if (fin != Fin.More)
                        {
                            errCode = "An unsupported opcode.";
                        }
                    }
                    break;
            }
            //----------------------------------------------------------
            if (errCode != null)
            {
                //report error
                throw new NotSupportedException();
            }
            //----------------------------------------------------------  
            this._currentPacketLen = payloadLen;
            currentReq.OpCode = currentOpCode;
            this._currentMaskLen = (currentMask == Mask.On) ? 4 : 0;
            //----------------------------------------------------------
            if (payloadLen >= 126)
            {
                this.parseState = ParseState.ReadExtendedPayloadLen;
                return true;
            }
            //----------------------------------------------------------
            this.parseState = this._currentMaskLen > 0 ?
                ParseState.ReadMask :
                ParseState.ExpectBody;
            return true;
        }
        bool ReadPayloadLen()
        {
            int extendedPayloadByteCount = (this._currentPacketLen == 126 ? 2 : 8);
            if (!myBufferStream.Ensure(extendedPayloadByteCount))
            {
                myBufferStream.BackupRecvIO();
                return false;
            }
            //----------------------------------------------------------

            myBufferStream.CopyBuffer(fullPayloadLengthBuffer, extendedPayloadByteCount);

            ulong org_packetLen1 = GetFullPayloadLength(_currentPacketLen, fullPayloadLengthBuffer);
            if (org_packetLen1 >= int.MaxValue)
            {
                //in this version ***
                throw new NotSupportedException();
            }
            this._currentPacketLen = (int)org_packetLen1;
            this.parseState = _currentMaskLen > 0 ?
                     ParseState.ReadMask :
                     ParseState.ExpectBody;
            return true;
        }
        bool ReadMask()
        {
            if (!myBufferStream.Ensure(_currentMaskLen))
            {
                myBufferStream.BackupRecvIO();
                return false;
            }
            //---------------------------------------------------------- 
            //read mask data                     

            myBufferStream.CopyBuffer(maskKey, _currentMaskLen);
            this.parseState = ParseState.ExpectBody;
            return true;
        }

        internal ProcessReceiveBufferResult ParseRecvData()
        {
            myBufferStream.AppendNewRecvData();

            for (;;)
            {

                switch (parseState)
                {
                    default:
                        throw new NotSupportedException();
                    case ParseState.Init:

                        if (!ReadHeader())
                        {
                            return ProcessReceiveBufferResult.NeedMore;
                        }
                        break;
                    case ParseState.ReadExtendedPayloadLen:

                        if (!ReadPayloadLen())
                        {
                            return ProcessReceiveBufferResult.NeedMore;
                        }
                        break;
                    case ParseState.ReadMask:

                        if (!ReadMask())
                        {
                            return ProcessReceiveBufferResult.NeedMore;
                        }

                        break;
                    case ParseState.ExpectBody:
                        {
                            //------------------------------------- 
                            switch (currentOpCode)
                            {
                                //ping,
                                //pong
                                default:
                                    throw new NotSupportedException();
                                case Opcode.Binary:
                                case Opcode.Text:
                                case Opcode.Close:
                                    break;
                            }

                            if (!ReadBodyContent(_currentPacketLen))
                            {
                                return ProcessReceiveBufferResult.NeedMore;
                            }


                            parseState = ParseState.Init;

                            if (myBufferStream.IsEnd())
                            {
                                myBufferStream.Clear();
                                return ProcessReceiveBufferResult.Complete;
                            }
                            //more than 1 byte 
                        }
                        break;
                }
            }
        }
        bool ReadBodyContent(int readLen)
        {
            if (!myBufferStream.Ensure(readLen))
            {
                myBufferStream.BackupRecvIO();
                return false;
            }
            //------------------------------------
            byte[] data = new byte[readLen];
            myBufferStream.CopyBuffer(data, readLen);
            if (useMask)
            {
                //unmask
                MaskAgain(data, maskKey);
            }
            currentReq.SetData(data);
            return true;
        }

        static ulong GetFullPayloadLength(int payloadLength, byte[] fullPayloadLengthBuffer)
        {
            // Payload length:  7 bits, 7+16 bits, or 7+64 bits

            //The length of the "Payload data", in bytes: if 0-125, that is the
            //payload length.  If 126, the following 2 bytes interpreted as a
            //16-bit unsigned integer are the payload length.  If 127, the
            //following 8 bytes interpreted as a 64-bit unsigned integer (the
            //most significant bit MUST be 0) are the payload length.  Multibyte
            //length quantities are expressed in network byte order.  Note that
            //in all cases, the minimal number of bytes MUST be used to encode
            //the length, for example, the length of a 124-byte-long string
            //can't be encoded as the sequence 126, 0, 124.  The payload length
            //is the length of the "Extension data" + the length of the
            //"Application data".  The length of the "Extension data" may be
            //zero, in which case the payload length is the length of the
            //"Application data". 
            return payloadLength < 126
                       ? (ulong)payloadLength //use that length
                       : payloadLength == 126
                         ? (ulong)(fullPayloadLengthBuffer[0] << 8 | fullPayloadLengthBuffer[1]) // 7+16 bits
                         : BitConverter.ToUInt64(fullPayloadLengthBuffer, 0);

        }

        static void MaskAgain(byte[] data, byte[] key)
        {

            for (int i = data.Length - 1; i >= 0; --i)
            {
                data[i] ^= key[i % 4];
            }
        }
    }


}