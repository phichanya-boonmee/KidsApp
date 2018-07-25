//2010, CPOL, Stan Kirk
//2015, MIT, EngineKit

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text; //for testing

namespace SharpConnect
{
    public delegate void ReqRespHandler<REQ, RESP>(REQ req, RESP resp);
}
