//2015, MIT, EngineKit

using System;
using System.Reflection;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.IO;


using SharpConnect.WebServers;
namespace SharpConnect
{


    //4.  
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    class HttpMethodAttribute : Attribute
    {
        public string AlternativeName { get; set; }
    }

    class AppHost
    {

        public string CurrentServerMsg = "";
        public int success = 0;
        public int failed = 0;
        public int requested = 0;
        //test cross origin policy 1
        static CrossOriginPolicy crossOriginPolicy = new CrossOriginPolicy(AllowCrossOrigin.All, "*");
        static AppHost()
        {
            //eg.
            //stBuilder.Append("Access-Control-Allow-Methods: GET, POST\r\n");
            //stBuilder.Append("Access-Control-Allow-Headers: Content-Type\r\n");
            crossOriginPolicy.AllowHeaders = "Content-Type";
            crossOriginPolicy.AllowMethods = "GET, POST";
        }

        const string html = @"<html>
                <head>
                <script> 
                         
                        var wsUri=get_wsurl(); 
                        var websocket= null;
                        (function init(){
	  
		                        //command queue 
		                        websocket = new WebSocket(wsUri);
		                        websocket.onopen = function(evt) { 
			                        console.log('open');
			                        websocket.send('client: Hello!');
		                        };
		                        websocket.onclose = function(evt) { 
			                        console.log('close');
		                        };
		                        websocket.onmessage = function(evt) {  
			                        console.log(evt.data);
		                        };
		                        websocket.onerror = function(evt) {  
		                        };		
                         })();
                        function send_data(data){
	                            websocket.send(data);
                        } 
                        function get_wsurl(){
                               
                                if(window.location.protocol==""https:""){
                                    return  ""wss://localhost:8000"";
                                }else{
                                    return  ""ws://localhost:8080"";
                                }
                        }
                </script>                
                </head>
                <body>
                        hello-websocket
	                    <input type=""button"" id=""mytxt"" onclick=""send_data('hello')""></input>	
                </body>    
        </html>";



        Dictionary<string, MethodInfo> httpMethods = new Dictionary<string, MethodInfo>();
        Dictionary<string, MetAdapter> httpCategories = new Dictionary<string, MetAdapter>();

        public void RegisterModule(object moduleInstance)
        {
            Type tt = moduleInstance.GetType();
            foreach (MethodInfo m in tt.GetMethods())
            {
                string typename = tt.Name;
                //m.Name;
                int count = 0;
                foreach (Attribute httpAttr in m.GetCustomAttributes(typeof(HttpMethodAttribute)))
                {

                    HttpMethodAttribute httpMethodAttr = (HttpMethodAttribute)httpAttr;
                    MetAdapter metAdapter = new MetAdapter(moduleInstance, m);


                    if (count == 0)
                    {
                        //httpMethods.Add("/" + m.Name.ToLower(), m);
                        //httpMethods.Add("/" + m.Name.ToLower(),r.methodName);
                        httpCategories.Add("/" + typename.ToLower() + "/" + metAdapter.metInfo.Name.ToLower(), metAdapter);
                    }
                    count++;
                    if (httpMethodAttr.AlternativeName != null)
                    {
                        //httpMethods.Add("/" + httpMethodAttr.AlternativeName.ToLower(), m);
                        httpCategories.Add("/" + typename.ToLower() + "/" + httpMethodAttr.AlternativeName.ToLower(), metAdapter);
                    }
                }
            }
        }

        
        public AppHost()
        {

        }
        
        public void HandleRequest(HttpRequest req, HttpResponse resp)
        {
            requested += 1;
            CurrentServerMsg = req.Url.ToString();
            //string rootFolder = @"C:\apache2\htdocs\";
            //string absFile = rootFolder + "\\" + req.Url;
            //if (File.Exists(absFile))
            //{
            //    byte[] buffer = File.ReadAllBytes(absFile);
            //    resp.AllowCrossOriginPolicy = crossOriginPolicy;
            //    switch (Path.GetExtension(absFile))
            //    {
            //        case ".jpg":
            //            resp.ContentType = WebResponseContentType.ImageJpeg;
            //            break;
            //        case ".png":
            //            resp.ContentType = WebResponseContentType.ImagePng;
            //            break;
            //        case ".php":
            //        case ".html":
            //            resp.ContentType = WebResponseContentType.TextHtml;
            //            break;
            //        case ".js":
            //            resp.ContentType = WebResponseContentType.TextJavascript;
            //            break;
            //        case ".css":
            //            resp.ContentType = WebResponseContentType.TextCss;
            //            break;
            //    }
            //    resp.End(buffer);
            //}
            //else
            //{
            //    resp.End("something wrong");
            //}
            resp.ContentType = WebResponseContentType.TextPlain;
            resp.AllowCrossOriginPolicy = crossOriginPolicy;
            
            string req_url = req.Url.ToLower();
            switch (req_url)
            {


                case "/check_msg":
                    {
                        resp.End(CurrentServerMsg);
                    }
                    break;
                case "/test.php":
                    {
                        resp.End("hello from php server!");
                    }
                    break;
                case "/":
                    {
                        resp.TransferEncoding = ResponseTransferEncoding.Chunked;
                        resp.End("hello!" + (count++));
                    }
                    break;
                case "/websocket":
                    {
                        resp.ContentType = WebResponseContentType.TextHtml;
                        resp.End(html);
                    }
                    break;
                case "/version":
                    {
                        resp.End("1.0");
                    }
                    break;
                case "/cross":
                    {
                        resp.AllowCrossOriginPolicy = crossOriginPolicy;
                        resp.End("ok");
                    }
                    break;
                default:
                    {
                        if (httpCategories.TryGetValue(req_url, out MetAdapter met))
                        {

                            //if (req.ReqParameters == null) //no request
                            //{
                            //    resp.End("Input error");
                            //}
                            //else
                            //{
                            //    met.Invoke(req, resp);
                            //}
                            
                            Console.WriteLine(req_url);
                            met.Invoke(req, resp);
                            success += 1;

                        }
                        else
                        {
                            failed += 1;
                            resp.End("?");
                        }

                        //if (req.Url.StartsWith("/mymodule/go"))
                        //{
                        //    //3.
                        //    myModule.Go(req, resp);
                        //}
                        //else if (req.Url.StartsWith("/mymodule/say"))
                        //{
                        //    myModule.Say(req, resp);
                        //}
                        //else
                        //{
                        //    resp.End("?");
                        //}

                    }
                    break;
            }
            
        }

        int count = 0;
        public void HandleWebSocket(WebSocketRequest req, WebSocketResponse resp)
        {
            resp.Write("server:" + (count++));
            
        }
    }
}