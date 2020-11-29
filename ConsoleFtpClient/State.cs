using System.Collections.Generic;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient
{
    public static class State
    {
        public static List<string> LocalPath = new List<string>();
        public static List<string> FtpPath = new List<string>()
        {
            ""
        };

        public static FileStruct[] FtpItems;
        public static FtpClient Client;
    }
}