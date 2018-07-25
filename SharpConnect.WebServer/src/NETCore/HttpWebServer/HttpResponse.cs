//2015-2016, MIT, EngineKit
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{
    /// <summary>
    /// content type
    /// </summary>
    public enum WebResponseContentType : byte
    {
        TextHtml,
        TextPlain,
        TextXml,
        TextJavascript,
        TextCss,

        ImagePng,
        ImageJpeg,

        ApplicationOctetStream,
        ApplicationJson,
    }

    public enum TextCharSet : byte
    {
        Ascii,
        Utf8
    }

    public enum ResponseTransferEncoding : byte
    {
        Identity,//no encoding
        Chunked,
        Gzip,
        Compress,
        Deflate
    }

    public enum ContentEncoding : byte
    {
        Plain,//no encoding
        Gzip,
        Deflate,
    }

    public enum AllowCrossOrigin : byte
    {
        None,
        All,
        Some
    }

    public class CrossOriginPolicy
    {
        public CrossOriginPolicy(AllowCrossOrigin allowKind, string originList)
        {
            this.AllowCrossOriginKind = allowKind;
            this.AllowOriginList = originList;

#if DEBUG
            if (allowKind == AllowCrossOrigin.Some && originList == null)
            {
                throw new NotSupportedException();
            }
#endif
        }
        public string AllowOriginList { get; private set; }
        public AllowCrossOrigin AllowCrossOriginKind { get; private set; }
        public string AllowMethods { get; set; }
        public string AllowHeaders { get; set; }
        internal void WriteHeader(StringBuilder stbuilder)
        {
            switch (AllowCrossOriginKind)
            {
                default:
                case AllowCrossOrigin.None:
                    return;
                case AllowCrossOrigin.All:
                    stbuilder.Append("Access-Control-Allow-Origin: *\r\n");
                    break;
                case AllowCrossOrigin.Some:
                    stbuilder.Append("Access-Control-Allow-Origin: ");
                    stbuilder.Append(AllowOriginList);
                    stbuilder.Append("\r\n");
                    break;
            }
            if (AllowMethods != null)
            {
                stbuilder.Append("Access-Control-Allow-Methods: ");
                stbuilder.Append(AllowMethods);
                stbuilder.Append("\r\n");
            }

            if (AllowHeaders != null)
            {
                stbuilder.Append("Access-Control-Allow-Headers: ");
                stbuilder.Append(AllowHeaders);
                stbuilder.Append("\r\n");
            }
        }
    }

    public class HttpResponse : IDisposable
    {
        enum WriteContentState : byte
        {
            HttpHead,
            HttpBody,
        }

        readonly HttpContext context;
        WriteContentState writeContentState;
        //output stream
        MemoryStream bodyMs;
        int contentByteCount;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        StringBuilder headerStBuilder = new StringBuilder();
        SendIO sendIO;


        internal HttpResponse(HttpContext context, SendIO sendIO)
        {
            this.context = context;
            bodyMs = new MemoryStream();
            StatusCode = 200; //init
            this.sendIO = sendIO;
            this.ContentTypeCharSet = WebServers.TextCharSet.Utf8;
        }
        public CrossOriginPolicy AllowCrossOriginPolicy
        {
            get;
            set;
        }
        internal void ResetAll()
        {
            headerStBuilder.Length = 0;
            StatusCode = 200;

            isSend = false;
            TransferEncoding = ResponseTransferEncoding.Identity;
            writeContentState = WriteContentState.HttpHead;
            ContentType = WebResponseContentType.TextPlain;//reset content type
            ContentEncoding = ContentEncoding.Plain;
            this.ContentTypeCharSet = TextCharSet.Utf8;
            AllowCrossOriginPolicy = null;
            headers.Clear();
            ResetWritingBuffer();
        }
        void ResetWritingBuffer()
        {
            bodyMs.Position = 0;
            contentByteCount = 0;
        }
        public void Dispose()
        {
            if (bodyMs != null)
            {
                bodyMs.Dispose();
                bodyMs = null;
            }
        }


        /// <summary>
        /// add new or replace if exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetHeader(string key, string value)
        {
            //replace exiting values
            //TODO: review custom header here
            headers[key] = value;
        }
        public WebResponseContentType ContentType
        {
            get;
            set;
        }
        public ContentEncoding ContentEncoding
        {
            get;
            set;
        }
        public TextCharSet ContentTypeCharSet
        {
            get;
            set;
        }

        /// <summary>
        /// write to output
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            //write to output stream 
            byte[] bytes = Encoding.UTF8.GetBytes(str.ToCharArray());
            //write to stream
            bodyMs.Write(bytes, 0, bytes.Length);
            contentByteCount += bytes.Length;
        }
        /// <summary>
        /// write to output
        /// </summary>
        /// <param name="str"></param>
        public void Write(byte[] rawBuffer)
        {   
            bodyMs.Write(rawBuffer, 0, rawBuffer.Length);
            contentByteCount += rawBuffer.Length;
        }
        public void End(string str)
        {
            //Write and End
            Write(str);
            End();
        }
        public void End(byte[] data)
        {
            bodyMs.Write(data, 0, data.Length);
            contentByteCount += data.Length;
            End();
        }
        public void End()
        {
            switch (writeContentState)
            {
                //generate head 
                case WriteContentState.HttpHead:
                    {
                        
                        headerStBuilder.Length = 0;
                        headerStBuilder.Append("HTTP/1.1 ");
                        HeaderAppendStatusCode(headerStBuilder, StatusCode);
                        HeaderAppendConnectionType(headerStBuilder, this.context.KeepAlive);
                        //--------------------------------------------------------------------------------------------------------
                        headerStBuilder.Append("Content-Type: " + GetContentType(this.ContentType));
                        switch (ContentTypeCharSet)
                        {
                            case TextCharSet.Utf8:
                                headerStBuilder.Append(" ; charset=utf-8\r\n");
                                break;
                            case TextCharSet.Ascii:
                                headerStBuilder.Append("\r\n");
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        //--------------------------------------------------------------------------------------------------------
                        switch (ContentEncoding)
                        {
                            case WebServers.ContentEncoding.Plain:
                                //nothing
                                break;
                            case WebServers.ContentEncoding.Gzip:
                                headerStBuilder.Append("Content-Encoding: gzip\r\n");
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        //--------------------------------------------------------------------------------------------------------
                        //Access-Control-Allow-Origin
                        if (AllowCrossOriginPolicy != null)
                        {
                            AllowCrossOriginPolicy.WriteHeader(headerStBuilder);
                        }
                        //--------------------------------------------------------------------------------------------------------
                        switch (this.TransferEncoding)
                        {
                            default:
                            case ResponseTransferEncoding.Identity:
                                {
                                    headerStBuilder.Append("Content-Length: ");
                                    headerStBuilder.Append(contentByteCount);
                                    headerStBuilder.Append("\r\n");
                                    //-----------------------------------------------------------------                                    
                                    headerStBuilder.Append("\r\n");//end header part                                     
                                    writeContentState = WriteContentState.HttpBody;
                                    //-----------------------------------------------------------------
                                    //switch transfer encoding method of the body***
                                    var headBuffer = Encoding.UTF8.GetBytes(headerStBuilder.ToString().ToCharArray());
                                    byte[] dataToSend = new byte[headBuffer.Length + contentByteCount];
                                    Buffer.BlockCopy(headBuffer, 0, dataToSend, 0, headBuffer.Length);
                                    var pos = bodyMs.Position;
                                    bodyMs.Position = 0;
                                    bodyMs.Read(dataToSend, headBuffer.Length, contentByteCount);
                                    //----------------------------------------------------
                                    //copy data to send buffer
                                    sendIO.EnqueueOutputData(dataToSend, dataToSend.Length);

                                    //---------------------------------------------------- 
                                    ResetAll();
                                }
                                break;
                            case ResponseTransferEncoding.Chunked:
                                {
                                    headerStBuilder.Append("Transfer-Encoding: " + GetTransferEncoding(TransferEncoding) + "\r\n");
                                    headerStBuilder.Append("\r\n");
                                    writeContentState = WriteContentState.HttpBody;

                                    //chunked transfer
                                    var headBuffer = Encoding.UTF8.GetBytes(headerStBuilder.ToString().ToCharArray());
                                    sendIO.EnqueueOutputData(headBuffer, headBuffer.Length);
                                    WriteContentBodyInChunkMode();
                                    ResetAll();
                                }
                                break;
                        }
                    }
                    break;
                //==============================
                case WriteContentState.HttpBody:
                    {
                        //in chunked case, 
                        WriteContentBodyInChunkMode();
                        ResetAll();
                    }
                    break;
                default:
                    {
                        throw new NotSupportedException();
                    }
            }

            //-----------------------
            //send 

            StartSend();
        }
        bool isSend = false;
        void StartSend()
        {
            if (isSend)
            {
                return;
            }
            isSend = true;
            sendIO.StartSendAsync();
        }
        void WriteContentBodyInChunkMode()
        {
            //---------------------------------------------------- 
            var pos = bodyMs.Position;
            bodyMs.Position = 0;
            byte[] bodyLengthInHex = Encoding.UTF8.GetBytes(contentByteCount.ToString("X"));
            int chuckedPrefixLength = bodyLengthInHex.Length;
            byte[] bodyBuffer = new byte[chuckedPrefixLength + contentByteCount + 4];
            int w = 0;
            Buffer.BlockCopy(bodyLengthInHex, 0, bodyBuffer, 0, chuckedPrefixLength);
            w += chuckedPrefixLength;
            bodyBuffer[w] = (byte)'\r';
            w++;
            bodyBuffer[w] = (byte)'\n';
            w++;
            bodyMs.Read(bodyBuffer, w, contentByteCount);
            w += contentByteCount;
            bodyBuffer[w] = (byte)'\r';
            w++;
            bodyBuffer[w] = (byte)'\n';
            w++;
            sendIO.EnqueueOutputData(bodyBuffer, bodyBuffer.Length);
            //---------------------------------------------------- 

            //end body
            byte[] endChuckedBlock = new byte[] { (byte)'0', (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n' };
            sendIO.EnqueueOutputData(endChuckedBlock, endChuckedBlock.Length);
            //---------------------------------------------------- 
            ResetWritingBuffer();
        }
        public ResponseTransferEncoding TransferEncoding
        {
            get;
            set;
        }
        internal int StatusCode
        {
            get;
            set;
        }

        //-------------------------------------------------
        static string GetTransferEncoding(ResponseTransferEncoding te)
        {
            switch (te)
            {
                case ResponseTransferEncoding.Chunked:
                    return "chunked";
                case ResponseTransferEncoding.Compress:
                    return "compress";
                case ResponseTransferEncoding.Deflate:
                    return "deflate";
                case ResponseTransferEncoding.Gzip:
                    return "gzip";
                default:
                    return "";
            }
        }

        static string GetContentType(WebResponseContentType contentType)
        {
            //TODO: review here again
            switch (contentType)
            {
                case WebResponseContentType.ImageJpeg:
                    return "image/jpeg";
                case WebResponseContentType.ImagePng:
                    return "image/png";
                case WebResponseContentType.ApplicationOctetStream:
                    return "application/octet-stream";
                case WebResponseContentType.ApplicationJson:
                    return "application/json";
                case WebResponseContentType.TextXml:
                    return "text/xml";
                case WebResponseContentType.TextHtml:
                    return "text/html";
                case WebResponseContentType.TextJavascript:
                    return "text/javascript";
                case WebResponseContentType.TextCss:
                    return "text/css";
                case WebResponseContentType.TextPlain:
                    return "text/plain";
                default:
                    throw new NotSupportedException();
            }
        }

        static void HeaderAppendConnectionType(StringBuilder headerStBuilder, bool keepAlive)
        {
            if (keepAlive)
                headerStBuilder.Append("Connection: keep-alive\r\n");
            else
                headerStBuilder.Append("Connection: close\r\n");
        }

        static void HeaderAppendStatusCode(StringBuilder stBuilder, int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    stBuilder.Append("200 OK\r\n");
                    return;
                case 500:
                    stBuilder.Append("500 InternalServerError\r\n");
                    return;
                default:
                    //from 'Nowin' project
                    stBuilder.Append((byte)('0' + statusCode / 100));
                    stBuilder.Append((byte)('0' + statusCode / 10 % 10));
                    stBuilder.Append((byte)('0' + statusCode % 10));
                    stBuilder.Append("\r\n");
                    return;
            }
        }



    }

}