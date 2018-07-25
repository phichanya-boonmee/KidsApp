//2010, CPOL, Stan Kirk
//2015, MIT, EngineKit

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace SharpConnect.Internal
{
    public sealed class NewConnListenerSettings
    {
#if DEBUG
        static NewConnListenerSettings()
        {
            dbugLOG.StartLog();

        }
#endif
        public NewConnListenerSettings(int maxConnections,
            int excessNumberOfSocketAsyncsInPool,
            int backlog,
            int maxSimultaneousAcceptOps, IPEndPoint listenerEndPoint)
        {
            this.MaxConnections = maxConnections;
            this.NumOfConnSession = maxConnections + excessNumberOfSocketAsyncsInPool;
            this.Backlog = backlog;
            this.MaxAcceptOps = maxSimultaneousAcceptOps;
            this.ListnerEndPoint = listenerEndPoint;

        }

        /// <summary>
        /// the maximum number of connections the sample is designed to handle simultaneously 
        /// </summary>
        public int MaxConnections { get; private set; }
        /// <summary>
        /// this variable allows us to create some extra SAEA objects for the pool,if we wish.         
        /// </summary>
        public int NumOfConnSession { get; private set; }
        /// <summary>
        ///  max number of pending connections the listener can hold in queue
        /// </summary>
        public int Backlog { get; private set; }
        /// <summary>
        /// tells us how many objects to put in pool for accept operations
        /// </summary>
        public int MaxAcceptOps { get; private set; }

        ///// <summary>
        ///// Endpoint for the listener.
        ///// </summary>
        public IPEndPoint ListnerEndPoint { get; private set; }
    }

}
