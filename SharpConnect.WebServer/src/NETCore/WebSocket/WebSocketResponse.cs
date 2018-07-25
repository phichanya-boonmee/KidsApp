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
using System.IO;
using System.Text;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{
    public class WebSocketResponse : IDisposable
    {
        MemoryStream bodyMs = new MemoryStream();
        readonly WebSocketContext conn;
        SendIO sendIO;
        internal WebSocketResponse(WebSocketContext conn, SendIO sendIO)
        {
            this.conn = conn;
            this.sendIO = sendIO;
        }
        public WebSocketContext OwnerContext
        {
            get { return this.conn; }
        }
        public void Dispose()
        {
            if (bodyMs != null)
            {
                bodyMs.Dispose();
                bodyMs = null;
            }
        }
        public void Write(string content)
        {
            byte[] dataToSend = CreateSendBuffer(content);
            sendIO.EnqueueOutputData(dataToSend, dataToSend.Length);
            sendIO.StartSendAsync();
        }
        public int SendQueueCount
        {
            get
            {
                return sendIO.QueueCount;
            }
        }

        static byte[] CreateSendBuffer(string msg)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                //create data   
                byte b1 = ((byte)Fin.Final) << 7; //final
                //// FIN
                //Fin fin = (b1 & (1 << 7)) == (1 << 7) ? Fin.Final : Fin.More; 
                //// RSV1
                //Rsv rsv1 = (b1 & (1 << 6)) == (1 << 6) ? Rsv.On : Rsv.Off;  //on compress
                //// RSV2
                //Rsv rsv2 = (b1 & (1 << 5)) == (1 << 5) ? Rsv.On : Rsv.Off; 
                //// RSV3
                //Rsv rsv3 = (b1 & (1 << 4)) == (1 << 4) ? Rsv.On : Rsv.Off; 

                //-------------
                //opcode: 1 = text
                b1 |= 1;
                //-------------


                byte[] dataToClient = Encoding.UTF8.GetBytes(msg);
                //if len <126  then               
                int dataLen = dataToClient.Length;

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

                ms.WriteByte(b1);

                if (dataLen < 126)
                {
                    byte b2 = (byte)dataToClient.Length; // < 126
                    //-----------------------------
                    //no extened payload length
                    //no mask key                    
                    ms.WriteByte(b2);
                }
                else if (dataLen < ushort.MaxValue)
                {
                    //If 126, the following 2 bytes interpreted as a
                    //16-bit unsigned integer are the payload length 
                    ms.WriteByte(126);
                    //use 2 bytes for dataLen  
                    ms.WriteByte((byte)(dataLen >> 8));
                    ms.WriteByte((byte)(dataLen & 0xff));
                }
                else
                {
                    //If 127, the
                    //following 8 bytes interpreted as a 64-bit unsigned integer (the
                    //most significant bit MUST be 0) are the payload length 
                    //this version we limit data len < int.MaxValue
                    if (dataLen > int.MaxValue)
                    {
                        throw new NotSupportedException();
                    }
                    //-----------------------------------------
                    ms.WriteByte(127);
                    //use 8 bytes for dataLen  
                    //so... first 4 bytes= 0

                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte((byte)((dataLen >> 24) & 0xff));
                    ms.WriteByte((byte)((dataLen >> 16) & 0xff));
                    ms.WriteByte((byte)((dataLen >> 8) & 0xff));
                    ms.WriteByte((byte)(dataLen & 0xff));
                }

                ms.Write(dataToClient, 0, dataToClient.Length);
                ms.Flush();
                //-----------------------------
                //mask : send to client no mask
                data = ms.ToArray();
                ms.Close();
            }

            return data;
        }
    }
}