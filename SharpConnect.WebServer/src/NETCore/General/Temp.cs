#if NET20
namespace System.Runtime.CompilerServices
{
    public partial class ExtensionAttribute : Attribute
    {
    }
}
#endif

namespace SharpConnect
{
    //TODO: review here
    //some methods are missing
    static class Temp
    {

        public static void Close(this System.IO.MemoryStream ms)
        {

        }
        public static void Close(this System.Net.Sockets.Socket socket)
        {

        }
        public static void Close(this System.IO.StreamWriter writer)
        {

        }
    }
}
