//2010, CPOL, Stan Kirk
//2015-2016, MIT, EngineKit 
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Diagnostics;

namespace SharpConnect.Internal
{
    /// <summary>
    /// new connection listener
    /// </summary>
    public class NewConnectionListener
    {
        // the socket used to listen for incoming connection requests
        Socket listenSocket;
        /// <summary>
        /// reusable AsyncEventArgs  pool for accept ops
        /// </summary>
        SharedResoucePool<SocketAsyncEventArgs> acceptArgsPool;
        NewConnListenerSettings setting;
        //A Semaphore has two parameters, the initial number of available slots
        //and the maximum number of slots. We'll make them the same. 
        //This Semaphore is used to keep from going over max connection number. (It is not about 
        //controlling threading really here.)   
        Semaphore maxConnEnforcer;

        /// <summary>
        /// handle new req connection
        /// </summary>
        Action<Socket> acceptNewConnectionHandler;

        public NewConnectionListener(NewConnListenerSettings setting, Action<Socket> acceptNewConnectionHandler)
        {
            this.setting = setting;
            this.acceptArgsPool = new SharedResoucePool<SocketAsyncEventArgs>(this.setting.MaxAcceptOps);
            this.acceptNewConnectionHandler = acceptNewConnectionHandler;
            // Create connections count enforcer
            this.maxConnEnforcer = new Semaphore(this.setting.MaxConnections, this.setting.MaxConnections);
        }
        /// <summary>
        /// async start listen for  new client
        /// </summary>
        public void StartListening()
        {
            InitPools();
            InitListenSocket();
            StartAccept();
        }

        void InitPools()
        {
            // preallocate pool of SocketAsyncEventArgs objects for accept operations           
            for (int i = this.setting.MaxAcceptOps - 1; i >= 0; --i)
            {
                // add SocketAsyncEventArg to the pool                 
                this.acceptArgsPool.Push(CreateSocketAsyncEventArgsForAccept());
            }
        }
        void InitListenSocket()
        {
#if DEBUG
            if (dbugLOG.enableDebugLog)
            {
                if (dbugLOG.watchProgramFlow)   //for testing
                {
                    dbugLOG.WriteLine("StartListen method. Before Listen operation is started.");
                }

            }
#endif
            // create the socket which listens for incoming connections
            listenSocket = new Socket(this.setting.ListnerEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //bind it to the port
            listenSocket.Bind(this.setting.ListnerEndPoint);

            // Start the listener with a backlog of however many connections.
            //"backlog" means pending connections. 
            //The backlog number is the number of clients that can wait for a
            //SocketAsyncEventArg object that will do an accept operation.
            //The listening socket keeps the backlog as a queue. The backlog allows 
            //for a certain # of excess clients waiting to be connected.
            //If the backlog is maxed out, then the client will receive an error when
            //trying to connect.
            //max # for backlog can be limited by the operating system.
            listenSocket.Listen(this.setting.Backlog);

//#if DEBUG
//            if (dbugLOG.watchProgramFlow)   //for testing
//            {
//                dbugLOG.WriteLine("StartListen method Listen operation was just started.");
//            }
//            Console.WriteLine("\r\n\r\n*************************\r\n** Server is listening **\r\n*************************\r\n\r\nAfter you are finished, type 'Z' and press\r\nEnter key to terminate the server process.\r\nIf you terminate it by clicking X on the Console,\r\nthen the log will NOT write correctly.\r\n");
//#endif
            // Calls the method which will post accepts on the listening socket.            
            // This call just occurs one time from this StartListen method. 
            // After that the StartAccept method will be called in a loop.


        }
        SocketAsyncEventArgs CreateSocketAsyncEventArgsForAccept()
        {

            // This method is called when we need to create a new SAEA object to do
            //accept operations. The reason to put it in a separate method is so that
            //we can easily add more objects to the pool if we need to.
            //You can do that if you do NOT use a buffer in the SAEA object that does
            //the accept operations.  
            //Allocate the SocketAsyncEventArgs object. 
            var acceptEventArg = new SocketAsyncEventArgs();
#if DEBUG
            acceptEventArg.UserToken = new dbugAcceptOpUserToken(acceptArgsPool.dbugGetNewTokenId() + 10000);
#endif

            //SocketAsyncEventArgs.Completed is an event, (the only event,) 
            //declared in the SocketAsyncEventArgs class.
            //See http://msdn.microsoft.com/en-us/library/system.net.sockets.socketasynceventargs.completed.aspx.
            //An event handler should be attached to the event within 
            //a SocketAsyncEventArgs instance when an asynchronous socket 
            //operation is initiated, otherwise the application will not be able 
            //to determine when the operation completes.
            //Attach the event handler, which causes the calling of the 
            //AcceptEventArg_Completed object when the accept op completes.
            acceptEventArg.Completed += (s, e) =>
            {
                // This method is the callback method associated with Socket.AcceptAsync 
                // operations and is invoked when an async accept operation completes.
                // This is only when a new connection is being accepted.
                // Notice that Socket.AcceptAsync is returning a value of true, and
                // raising the Completed event when the AcceptAsync method completes. 
                //Any code that you put in this method will NOT be called if
                //the operation completes synchronously, which will probably happen when
                //there is some kind of socket error. It might be better to put the code
                //in the ProcessAccept method. 
#if DEBUG
                dbugAcceptLog(e, "AcceptIO_Completed");
#endif
                ProcessAccept(e);
            };

            return acceptEventArg;
            //accept operations do NOT need a buffer.   ****             
            //You can see that is true by looking at the
            //methods in the .NET Socket class on the Microsoft website. AcceptAsync does
            //not take require a parameter for buffer size.
        }

        /// <summary>
        /// Begins an operation to accept a connection request from the client         
        /// </summary>
        void StartAccept()
        {
#if DEBUG
            if (dbugLOG.watchProgramFlow)   //for testing
            {
                dbugLOG.WriteLine("StartAccept method");
            }
#endif
            SocketAsyncEventArgs acceptEventArg;
            //Get a SocketAsyncEventArgs object to accept the connection.                        
            //Get it from the pool if there is more than one in the pool.
            //We could use zero as bottom, but one is a little safer.            
            if (this.acceptArgsPool.Count > 1)
            {
                try
                {
                    //use from pool
                    acceptEventArg = this.acceptArgsPool.Pop();
                }
                //or make a new one.
                catch
                {
                    acceptEventArg = CreateSocketAsyncEventArgsForAccept();
                }
            }
            //or make a new one.
            else
            {
                acceptEventArg = CreateSocketAsyncEventArgsForAccept();
            }


#if DEBUG
            var acceptToken = (dbugAcceptOpUserToken)acceptEventArg.UserToken;
            if (dbugLOG.enableDebugLog)
            {

                if (dbugLOG.watchThreads)   //for testing
                {
                    //dbugLOG.dbugDealWithThreadsForTesting("StartAccept()", acceptToken);
                }
                if (dbugLOG.watchProgramFlow)   //for testing
                {
                    dbugLOG.WriteLine("still in StartAccept, id = " + acceptToken.dbugTokenId);
                }
            }
#endif
            //Semaphore class is used to control access to a resource or pool of 
            //resources. Enter the semaphore by calling the WaitOne method, which is 
            //inherited from the WaitHandle class, and release the semaphore 
            //by calling the Release method. This is a mechanism to prevent exceeding
            // the max # of connections we specified. We'll do this before
            // doing AcceptAsync. If maxConnections value has been reached,
            //then the application will pause here until the Semaphore gets released,
            //which happens in the CloseClientSocket method.            
            this.maxConnEnforcer.WaitOne();

            //Socket.AcceptAsync begins asynchronous operation to accept the connection.
            //Note the listening socket will pass info to the SocketAsyncEventArgs
            //object that has the Socket that does the accept operation.
            //If you do not create a Socket object and put it in the SAEA object
            //before calling AcceptAsync and use the AcceptSocket property to get it,
            //then a new Socket object will be created for you by .NET.            

            //Socket.AcceptAsync returns true if the I/O operation is pending, i.e. is 
            //working asynchronously. The 
            //SocketAsyncEventArgs.Completed event on the acceptEventArg parameter 
            //will be raised upon completion of accept op.
            //AcceptAsync will call the AcceptEventArg_Completed
            //method when it completes, because when we created this SocketAsyncEventArgs
            //object before putting it in the pool, we set the event handler to do it.
            //AcceptAsync returns false if the I/O operation completed synchronously.            
            //The SocketAsyncEventArgs.Completed event on the acceptEventArg 
            //parameter will NOT be raised when AcceptAsync returns false.
            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
#if DEBUG
                if (dbugLOG.enableDebugLog && dbugLOG.watchProgramFlow)   //for testing
                {

                    dbugLOG.WriteLine("StartAccept in if (!willRaiseEvent), accept token id " + acceptToken.dbugTokenId);
                }
#endif

                //The code in this if (!willRaiseEvent) statement only runs 
                //when the operation was completed synchronously.
                //It is needed because 
                //when Socket.AcceptAsync returns false, 
                //it does NOT raise the SocketAsyncEventArgs.Completed event.
                //And we need to call ProcessAccept and pass it the SAEA object.
                //This is only when a new connection is being accepted.
                // Probably only relevant in the case of a socket error.
                ProcessAccept(acceptEventArg);
            }
        }

        void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {

            //The e parameter passed from the AcceptEventArg_Completed method
            //represents the SocketAsyncEventArgs object that did
            //the accept operation. in this method we'll do the handoff from it to the 
            //SocketAsyncEventArgs object that will do receive/send.

            // This is when there was an error with the accept op. That should NOT
            // be happening often. It could indicate that there is a problem with
            // that socket. If there is a problem, then we would have an infinite
            // loop here, if we tried to reuse that same socket.
            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                // Loop back to post another accept op. Notice that we are NOT ***
                // passing the SAEA object here.      
                dbugAcceptLog(acceptEventArgs, "SocketError");
                StartAccept(); //start accept again *** 
                //Let's destroy this socket, since it could be bad.
                HandleBadAccept(acceptEventArgs);
                //Jump out of the method.
                return;
            }
            //----------------------------------------------------------------------------------------------- 

#if DEBUG

            dbugAcceptLog(acceptEventArgs, "processAccept, then startAccept");
#endif

            //Now that the accept operation completed, we can start another
            //accept operation, which will do the same. 
            //Notice that we are NOT passing the SAEA object here.  
            StartAccept(); //start accept again ***
            //----------------------------------------------------------------------             

            //A new socket was created by the AcceptAsync method. The 
            //SocketAsyncEventArgs object which did the accept operation has that 
            //socket info in its AcceptSocket property. Now we will give
            //a reference for that socket to the SocketAsyncEventArgs 
            //object which will do receive/send.
            //----------------------------------------------------------------------
            acceptNewConnectionHandler(acceptEventArgs.AcceptSocket);//*** (move socket object from acceptArgs to recvSendArgs 
            //---------------------------------------------------------------------- 
            //We have handed off the connection info from the
            //accepting socket to the receiving socket. So, now we can
            //put the SocketAsyncEventArgs object that did the accept operation 
            //back in the pool for them. But first we will clear 
            //the socket info from that object, so it will be 
            //ready for a new socket when it comes out of the pool.
            acceptEventArgs.AcceptSocket = null; //after remove set this to null
            this.acceptArgsPool.Push(acceptEventArgs); //return to pool

#if DEBUG
            dbugAcceptLog(acceptEventArgs, "back to poolOfAcceptEventArgs");
#endif



        }
        void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {

            dbugAcceptLog(acceptEventArgs, "Closing socket of accept");

            //This method closes the socket and releases all resources, both
            //managed and unmanaged. It internally calls Dispose.           
            acceptEventArgs.AcceptSocket.Close();
            acceptEventArgs.AcceptSocket = null;
            //Put the SAEA back in the pool.
            acceptArgsPool.Push(acceptEventArgs);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        static void dbugAcceptLog(SocketAsyncEventArgs acceptEventArgs, string logMsg)
        {
#if DEBUG
            if (dbugLOG.enableDebugLog && dbugLOG.watchProgramFlow)   //for testing
            {
                var acceptOpToken = acceptEventArgs.UserToken as dbugAcceptOpUserToken;
                dbugLOG.WriteLine("acc_log:" + acceptOpToken.dbugTokenId + " " + logMsg);
            }
#endif
        }


        public void NotifyFreeAcceptQuota()
        {
            //Release Semaphore so that its connection counter will be decremented.
            //This must be done AFTER putting the SocketAsyncEventArg back into the pool,
            //or you can run into problems.   
            maxConnEnforcer.Release();
        }
        public void StopAccept()
        {
            //close listen socket
            listenSocket.Close();
        }
        public void DisposePool()
        {
            while (this.acceptArgsPool.Count > 0)
            {
                acceptArgsPool.Pop().Dispose();
            }
        }
    }

}