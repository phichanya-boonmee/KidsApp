//2010, CPOL, Stan Kirk
//2015, MIT, EngineKit

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SharpConnect.Internal
{
    class BufferManager
    {

        //Buffers for sockets are unmanaged by .NET. 
        //So memory used for buffers gets "pinned", which makes the
        //.NET garbage collector work around it, fragmenting the memory. 
        //Circumvent this problem by putting all buffers together 
        //in one block in memory. Then we will assign a part of that space 
        //to each SocketAsyncEventArgs object, and
        //reuse that buffer space each time we reuse the SocketAsyncEventArgs object.
        //Create a large reusable set of buffers for all socket operations.
        //---------------------------------------------------------------------------------

        // Allocate one large byte buffer block, which all I/O operations will 
        //use a piece of. This gaurds against memory fragmentation.
        //---------------------------------------------------------------------------------

        // This class creates a single large buffer which can be divided up 
        // and assigned to SocketAsyncEventArgs objects for use with each 
        // socket I/O operation.  
        // This enables buffers to be easily reused and guards against 
        // fragmenting heap memory.
        // 
        //This buffer is a byte array which the Windows TCP buffer can copy its data to.

        // the total number of bytes controlled by the buffer pool
        Int32 totalBytesInBufferBlock;

        // Byte array maintained by the Buffer Manager.
        byte[] bufferBlock;
        Stack<int> freeIndexPool;
        int currentIndex;
        int totalBufferBytesInEachSocketAsyncEventArgs;

        public BufferManager(int totalBytes, int totalBufferBytesInEachSocketAsyncEventArgs)
        {
            totalBytesInBufferBlock = totalBytes;
            this.currentIndex = 0;
            this.totalBufferBytesInEachSocketAsyncEventArgs = totalBufferBytesInEachSocketAsyncEventArgs;
            this.freeIndexPool = new Stack<int>();

            this.bufferBlock = new byte[totalBytesInBufferBlock];
        }
        // Divide that one large buffer block out to each SocketAsyncEventArg object.
        // Assign a buffer space from the buffer block to the 
        // specified SocketAsyncEventArgs object.
        //
        // returns true if the buffer was successfully set, else false
        /// <summary>
        /// add new buffer to args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal bool SetBufferFor(SocketAsyncEventArgs args)
        {

            if (this.freeIndexPool.Count > 0)
            {
                //This if-statement is only true if you have called the FreeBuffer
                //method previously, which would put an offset for a buffer space 
                //back into this stack.
                args.SetBuffer(this.bufferBlock, this.freeIndexPool.Pop(), this.totalBufferBytesInEachSocketAsyncEventArgs);
            }
            else
            {
                //Inside this else-statement is the code that is used to set the 
                //buffer for each SAEA object when the pool of SAEA objects is built
                //in the Init method.
                if ((totalBytesInBufferBlock - this.totalBufferBytesInEachSocketAsyncEventArgs) < this.currentIndex)
                {
                    return false;
                }
                args.SetBuffer(this.bufferBlock, this.currentIndex, this.totalBufferBytesInEachSocketAsyncEventArgs);
                this.currentIndex += this.totalBufferBytesInEachSocketAsyncEventArgs;
            }
            return true;
        }

        // Removes the buffer from a SocketAsyncEventArg object.   This frees the
        // buffer back to the buffer pool. Try NOT to use the FreeBuffer method,
        // unless you need to destroy the SAEA object, or maybe in the case
        // of some exception handling. Instead, on the server
        // keep the same buffer space assigned to one SAEA object for the duration of
        // this app's running.
        internal void FreeBuffer(SocketAsyncEventArgs args)
        {
            this.freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
        //-------------------------------------------------------------------------------------------------------


    }

}
