﻿namespace ConsoleFtpClient.Core
{
    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public bool IsDirectory;
        public string CreateTime;
        public string Name;
        public override string ToString()
        {
            return $"{Name} {Flags}";
        }
    }
}