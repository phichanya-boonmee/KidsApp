//2010, CPOL, Stan Kirk
//2015, MIT, EngineKit

using System;
using System.Net;
using System.Text;
using System.Threading;

namespace SharpConnect.Internal
{
#if DEBUG
    static class dbugLOG
    {
        //object that will be used to lock the listOfDataHolders
        static readonly object lockerForList = new object();

        //If you make this true, then the IncomingDataPreparer will not write to
        // a List<T>, and you will not see the printout of the data at the end
        //of the log.
        public static readonly bool runLongTest = false;

        //If you make this true, then info about threads will print to log.
        public static readonly bool watchThreads = false;

        //If you make this true, then the above "watch-" variables will print to
        //both Console and log, instead of just to log. I suggest only using this if
        //you are having a problem with an application that is crashing.
        public static readonly bool consoleWatch = false;


        //static List<DataHolder> dbugDataHolderList; 

        //This is for logging during testing.        
        //You can change the path in the TestFileWriter class if you need to.

        //If you make this a positive value, it will simulate some delay on the
        //receive/send SAEA object after doing a receive operation.
        //That would be where you would do some work on the received data, 
        //before responding to the client.
        //This is in milliseconds. So a value of 1000 = 1 second delay.
        public static readonly Int32 msDelayAfterGettingMessage = -1;

        public static bool enableDebugLog = false;

        //If this is true, then info about which method the program is in
        //will print to log.               
        public static bool watchProgramFlow = true;

        //If you make this true, then connect/disconnect info will print to log.
        public static readonly bool watchConnectAndDisconnect = true;

        //If you make this true, then data will print to log.        
        //public static readonly bool watchData = true;
        static dbugTestFileWriter testWriter;

        // To keep a record of maximum number of simultaneous connections
        // that occur while the server is running. This can be limited by operating
        // system and hardware. It will not be higher than the value that you set
        // for maxNumberOfConnections.
        public static Int32 maxSimultaneousClientsThatWereConnected = 0;

        //These strings are just for console interaction.
        public const string checkString = "C";
        public const string closeString = "Z";
        public const string wpf = "T";
        public const string wpfNo = "F";
        public static string wpfTrueString = "";
        public static string wpfFalseString = "";

        static object lockStart = new object();
        static bool isInit = false;
        internal static void StartLog()
        {
            lock (lockStart)
            {
                if (!isInit)
                {
                    //init once 
                    BuildStringsForServerConsole();
                    testWriter = new dbugTestFileWriter();
                    isInit = true;
                }
            }
        }


        ///// <summary>
        ///// /Display thread info.,Use this one in method where AcceptOpUserToken is available.
        ///// </summary>
        ///// <param name="methodName"></param>
        ///// <param name="acceptToken"></param>
        //public static void dbugDealWithThreadsForTesting(SocketServer socketServer, string methodName, dbugAcceptOpUserToken acceptToken)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string hString = hString = ". Socket handle " + acceptToken.dbugSocketHandleNumber;
        //    sb.Append(" In " + methodName + ", acceptToken id " + acceptToken.dbugTokenId + ". Thread id " + Thread.CurrentThread.ManagedThreadId + hString + ".");
        //    sb.Append(socketServer.dbugDealWithNewThreads());
        //    dbugLOG.WriteLine(sb.ToString());
        //}

        [System.Diagnostics.Conditional("DEBUG")]
        public static void dbugLog(string msg)
        {

            if (dbugLOG.enableDebugLog && dbugLOG.watchProgramFlow)   //for testing
            {
                dbugLOG.WriteLine(msg);
            }
        }
        internal static void WriteLine(string str)
        {
            testWriter.WriteLine(str);
        }
        static void BuildStringsForServerConsole()
        {
            if (dbugLOG.enableDebugLog)
            {
                StringBuilder sb = new StringBuilder();

                // Make the string to write.
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append("To take any of the following actions type the \r\ncorresponding letter below and press Enter.\r\n");
                sb.Append(closeString);
                sb.Append(")  to close the program\r\n");
                sb.Append(checkString);
                sb.Append(")  to check current status\r\n");
                string tempString = sb.ToString();
                sb.Length = 0;

                // string when watchProgramFlow == true 
                sb.Append(wpfNo);
                sb.Append(")  to quit writing program flow. (ProgramFlow is being logged now.)\r\n");
                wpfTrueString = tempString + sb.ToString();
                sb.Length = 0;

                // string when watchProgramFlow == false
                sb.Append(wpf);
                sb.Append(")  to start writing program flow. (ProgramFlow is NOT being logged.)\r\n");
                wpfFalseString = tempString + sb.ToString();
            }
        }


        internal static void WriteSetupInfo(IPEndPoint localEndPoint)
        {
            //Console.WriteLine("The following options can be changed in Program.cs file.");
            //Console.WriteLine("server buffer size = " + testBufferSize);
            //Console.WriteLine("max connections = " + maxNumberOfConnections);
            //Console.WriteLine("backlog variable value = " + backlog);
            //Console.WriteLine("watchProgramFlow = " + dbugWatchProgramFlow);
            //Console.WriteLine("watchConnectAndDisconnect = " + watchConnectAndDisconnect);
            //Console.WriteLine("watchThreads = " + dbugWatchThreads);
            //Console.WriteLine("msDelayAfterGettingMessage = " + dbugMsDelayAfterGettingMessage);
            if (dbugLOG.enableDebugLog)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("local endpoint = " + IPAddress.Parse(((IPEndPoint)localEndPoint).Address.ToString()) + ": " + ((IPEndPoint)localEndPoint).Port.ToString());
                Console.WriteLine("server machine name = " + Environment.MachineName);

                Console.WriteLine();
                Console.WriteLine("Client and server should be on separate machines for best results.");
                Console.WriteLine("And your firewalls on both client and server will need to allow the connection.");
                Console.WriteLine();
            }
        }


        static void WriteLogData()
        {
            //if ((watchData) && (runLongTest))
            //{
            //    Program.testWriter.WriteLine("\r\n\r\nData from DataHolders in listOfDataHolders follows:\r\n");
            //    int listCount = dbugDataHolderList.Count;
            //    for (int i = 0; i < listCount; i++)
            //    {   
            //        //DataHolder dataHolder = dbugDataHolderList[i];
            //        //Program.testWriter.WriteLine(IPAddress.Parse(((IPEndPoint)dataHolder.remoteEndpoint).Address.ToString()) + ": " +
            //        //    ((IPEndPoint)dataHolder.remoteEndpoint).Port.ToString() + ", " + dataHolder.receivedTransMissionId + ", " +
            //        //    Encoding.ASCII.GetString(dataHolder.dataMessageReceived));
            //    }
            //}
            //testWriter.WriteLine("\r\nHighest # of simultaneous connections was " + maxSimultaneousClientsThatWereConnected);
            //testWriter.WriteLine("# of transmissions received was " + (mainTransMissionId - startingTid));
        }
    }

#endif
}
