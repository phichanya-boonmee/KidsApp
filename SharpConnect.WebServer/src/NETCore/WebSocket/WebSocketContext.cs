//2015-2016, MIT, EngineKit
/* The MIT License
*
* Copyright (c) 2013-2015 sta.blockhead
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
using System.Net.Sockets;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{
    public class WebSocketContext : IDisposable
    {
        readonly WebSocketServer webSocketServer;
        readonly SocketAsyncEventArgs sockAsyncSender;
        readonly SocketAsyncEventArgs sockAsyncListener;

        ReqRespHandler<WebSocketRequest, WebSocketResponse> webSocketReqHandler;
        Socket clientSocket;

        const int RECV_BUFF_SIZE = 1024;

        WebSocketResponse webSocketResp;
        WebSocketProtocolParser webSocketReqParser;

        RecvIO recvIO;
        SendIO sendIO;
        int connectionId;
        static int connectionIdTotal;

        public WebSocketContext(WebSocketServer webSocketServer)
        {

            this.webSocketServer = webSocketServer;
            connectionId = System.Threading.Interlocked.Increment(ref connectionIdTotal);
            //-------------------
            //send,resp 
            sockAsyncSender = new SocketAsyncEventArgs();
            sockAsyncSender.SetBuffer(new byte[RECV_BUFF_SIZE], 0, RECV_BUFF_SIZE);
            sendIO = new SendIO(sockAsyncSender, 0, RECV_BUFF_SIZE, sendIO_SendCompleted);
            sockAsyncSender.Completed += new EventHandler<SocketAsyncEventArgs>((s, e) =>
            {
                switch (e.LastOperation)
                {
                    default:
                        {
                        }
                        break;
                    case SocketAsyncOperation.Send:
                        {
                            sendIO.ProcessWaitingData();
                        }
                        break;
                    case SocketAsyncOperation.Receive:
                        {
                        }
                        break;
                }
            });
            webSocketResp = new WebSocketResponse(this, sendIO);

            //------------------------------------------------------------------------------------
            //recv,req ,new socket
            sockAsyncListener = new SocketAsyncEventArgs();
            sockAsyncListener.SetBuffer(new byte[RECV_BUFF_SIZE], 0, RECV_BUFF_SIZE);
            recvIO = new RecvIO(sockAsyncListener, 0, RECV_BUFF_SIZE, HandleReceivedData);
            sockAsyncListener.Completed += new EventHandler<SocketAsyncEventArgs>((s, e) =>
            {
                switch (e.LastOperation)
                {
                    default:
                        {
                        }
                        break;
                    case SocketAsyncOperation.Send:
                        {
                        }
                        break;
                    case SocketAsyncOperation.Receive:
                        {
                            recvIO.ProcessReceivedData();
                        }
                        break;
                }
            });
            //------------------------------------------------------------------------------------             
            this.webSocketReqParser = new WebSocketProtocolParser(this, recvIO);

        }

        public void Bind(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            //sender
            sockAsyncSender.AcceptSocket = clientSocket;
            //------------------------------------------------------
            //listener   
            sockAsyncListener.AcceptSocket = clientSocket;
            //sockAsyncListener.SetBuffer(new byte[RECV_BUFF_SIZE], 0, RECV_BUFF_SIZE);
            //------------------------------------------------------
            //when bind we start listening 
            clientSocket.ReceiveAsync(sockAsyncListener);
            //------------------------------------------------------  
        }
        void HandleReceivedData(RecvEventCode recvCode)
        {
            switch (recvCode)
            {
                case RecvEventCode.HasSomeData:

                    //parse recv msg
                    switch (this.webSocketReqParser.ParseRecvData())
                    {
                        //in this version all data is copy into WebSocketRequest
                        //so we can reuse recv buffer 
                        //TODO: review this, if we need to copy?,  

                        case ProcessReceiveBufferResult.Complete:
                            {
                                //you can choose ...
                                //invoke webSocketReqHandler in this thread or another thread
                                while (webSocketReqParser.ReqCount > 0)
                                {
                                    WebSocketRequest req = webSocketReqParser.Dequeue();
                                    webSocketReqHandler(req, webSocketResp);
                                }
                                recvIO.StartReceive();
                                //***no code after StartReceive***
                            }
                            return;
                        case ProcessReceiveBufferResult.NeedMore:
                            {
                                recvIO.StartReceive();
                                //***no code after StartReceive***
                            }
                            return;
                        case ProcessReceiveBufferResult.Error:
                        default:
                            throw new NotSupportedException();
                    }

                case RecvEventCode.NoMoreReceiveData:
                    {
                    }
                    break;
                case RecvEventCode.SocketError:
                    {
                    }
                    break;
            }
        }
        void sendIO_SendCompleted(SendIOEventCode eventCode)
        {

        }
        public string Name
        {
            get;
            set;
        }
        public void Dispose()
        {

        }

        public int ConnectionId
        {
            get { return this.connectionId; }
        }
        public void SetMessageHandler(ReqRespHandler<WebSocketRequest, WebSocketResponse> webSocketReqHandler)
        {
            this.webSocketReqHandler = webSocketReqHandler;
        }


        public void Close()
        {
            clientSocket.Close();
        }
        public void Send(string dataToSend)
        {
            //send data to server
            //and wait for result 
            webSocketResp.Write(dataToSend);
        }
        public int SendQueueCount
        {
            get { return webSocketResp.SendQueueCount; }
        }
        internal void SendExternalRaw(byte[] data)
        {
            sendIO.EnqueueOutputData(data, data.Length);
            sendIO.StartSendAsync();
        }
        //---------------------------------------------
        public string InitClientRequestUrl
        {
            get;
            set;
        }
    }

}