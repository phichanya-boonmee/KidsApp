//2010, CPOL, Stan Kirk
//2015, MIT, EngineKit

using System;
namespace SharpConnect.Internal
{
#if DEBUG
    class dbugAcceptOpUserToken
    {
        //The only reason to use this UserToken in our app is to give it an identifier,
        //so that you can see it in the program flow. 
        //Otherwise, you would not need it.***  
        Int32 _dbugTokenId; //for testing only
        internal Int32 dbugSocketHandleNumber; //for testing only 
        public dbugAcceptOpUserToken(Int32 tokenId)
        {
            _dbugTokenId = tokenId;
        }
        public Int32 dbugTokenId
        {
            get
            {
                return _dbugTokenId;
            }
        }
    }
#endif
}
