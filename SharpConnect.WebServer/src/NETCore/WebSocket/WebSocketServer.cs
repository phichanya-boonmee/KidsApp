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
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{


    public class WebSocketServer
    {

        
        Action<WebSocketContext> newContextConnected;
        Dictionary<int, WebSocketContext> workingWebSocketConns = new Dictionary<int, WebSocketContext>();
        public WebSocketServer()
        {
            
        }
        internal WebSocketContext RegisterNewWebSocket(
            Socket clientSocket,
            string initUrl,
            string sec_websocket_key)
        {
            WebSocketContext wbcontext = new WebSocketContext(this );
            workingWebSocketConns.Add(wbcontext.ConnectionId, wbcontext);//add to working socket 
            wbcontext.Bind(clientSocket); //move client socket to webSocketConn    
            wbcontext.SendExternalRaw(MakeWebSocketUpgradeResponse(MakeResponseMagicCode(sec_websocket_key)));
            wbcontext.InitClientRequestUrl = initUrl;
            if (newContextConnected != null)
            {
                newContextConnected(wbcontext);
            }

            return wbcontext;
        }
        public void SetOnNewConnectionContext(Action<WebSocketContext> newContextConnected)
        {
            this.newContextConnected = newContextConnected;
        }
        static byte[] MakeWebSocketUpgradeResponse(string webSocketSecCode)
        {
            int contentByteCount = 0; // "" empty string 
            StringBuilder headerStBuilder = new StringBuilder();
            headerStBuilder.Length = 0;
            headerStBuilder.Append("HTTP/1.1 ");
            headerStBuilder.Append("101 Switching Protocols\r\n");
            headerStBuilder.Append("Upgrade: websocket\r\n");
            headerStBuilder.Append("Connection: Upgrade\r\n");
            headerStBuilder.Append("Sec-WebSocket-Accept: " + webSocketSecCode + "\r\n");
            headerStBuilder.Append("Content-Length: " + contentByteCount + "\r\n");
            headerStBuilder.Append("\r\n");

            //-----------------------------------------------------------------
            //switch transfer encoding method of the body***
            var headBuffer = Encoding.UTF8.GetBytes(headerStBuilder.ToString().ToCharArray());
            byte[] dataToSend = new byte[headBuffer.Length + contentByteCount];
            Buffer.BlockCopy(headBuffer, 0, dataToSend, 0, headBuffer.Length);
            return dataToSend;
        }
        //----------------------------
        //websocket
        const string magicString = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        static string MakeResponseMagicCode(string reqMagicString)
        {
            string total = reqMagicString + magicString;
            var sha1 = SHA1.Create();
            byte[] shaHash = sha1.ComputeHash(Encoding.ASCII.GetBytes(total));
            return Convert.ToBase64String(shaHash);
        }
    }


}