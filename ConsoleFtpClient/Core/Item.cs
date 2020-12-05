namespace ConsoleFtpClient.Core
{
    public struct Item
    {
        public string Name;
        public string FullLink;
        public bool IsDirectory;
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}