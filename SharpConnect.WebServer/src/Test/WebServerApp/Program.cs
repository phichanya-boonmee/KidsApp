//2015, MIT, EngineKit
using System;
using System.Net;
using System.Text;

using SharpConnect.WebServers;

namespace SharpConnect
{
    static class Program
    {

        static void Main(string[] args)
        {
            Main2();
        }
        static void Main2()
        {
            Console.WriteLine("Hello!, from SharpConnect");

            AppHost testApp = new AppHost();
            try
            {
                for (int i = 0; i < 1; ++i)
                {
                    //1. create  
                    WebServer webServer = new WebServer(8080 + i, true, testApp.HandleRequest);
                    //test websocket 
                    var webSocketServer = new WebSocketServer();
                    webSocketServer.SetOnNewConnectionContext(ctx =>
                    {
                        ctx.SetMessageHandler(testApp.HandleWebSocket);
                    });
                    webServer.WebSocketServer = webSocketServer;
                    webServer.Start();
                }


                string cmd = "";
                while (cmd != "X")
                {
                    cmd = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // close the stream for test file writing
                try
                {
#if DEBUG

#endif
                }
                catch
                {
                    Console.WriteLine("Could not close log properly.");
                }

            }
        }




        //        static void Main1()
        //        {

        //            SocketServerSettings serverSetting;
        //            TestApp testApp;
        //            int port = 4444;
        //            int maxNumberOfConnections = 1000;
        //            int excessSaeaObjectsInPool = 200;
        //            int backlog = 100;
        //            int maxSimultaneousAcceptOps = 100;
        //            testApp = new TestApp();

        //            try
        //            {
        //                // Get endpoint for the listener.                
        //                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
        //#if DEBUG
        //                dbugLOG.WriteSetupInfo(localEndPoint);
        //#endif
        //                //This object holds a lot of settings that we pass from Main method
        //                //to the SocketListener. In a real app, you might want to read
        //                //these settings from a database or windows registry settings that
        //                //you would create. 

        //                //---------------------------------------------------------------------------------
        //                //instantiate the SocketListener,
        //                //and Run Listen Loop in ctor *** 
        //                HttpSocketServerSetting setting = new HttpSocketServerSetting(
        //                    maxNumberOfConnections,
        //                    excessSaeaObjectsInPool,
        //                    backlog,
        //                    maxSimultaneousAcceptOps,
        //                    localEndPoint);
        //                setting.RequestHandler += testApp.HandleRequest;
        //                serverSetting = setting;
        //                //serverSetting = new CustomSocketServerSetting(maxNumberOfConnections,
        //                //   excessSaeaObjectsInPool,
        //                //   backlog,
        //                //   maxSimultaneousAcceptOps,
        //                //   testBufferSize,
        //                //   opsToPreAlloc,
        //                //   localEndPoint);
        //                //create and run until stop...
        //                SocketServer socketServer = new SocketServer(serverSetting);
        //                //run  
        //                //--------------------------------------------------------------------------------- 
        //#if DEBUG
        //                dbugLOG.ManageClosing(socketServer);
        //#endif
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //            }
        //            finally
        //            {
        //                // close the stream for test file writing
        //                try
        //                {
        //                    //#if DEBUG
        //                    //                    dbugLOG.testWriter.Close();
        //                    //#endif
        //                }
        //                catch
        //                {
        //                    Console.WriteLine("Could not close log properly.");
        //                }
        //            }
        //        }
    }
}