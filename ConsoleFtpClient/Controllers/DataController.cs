using System.Collections.Generic;
using ConsoleFtpClient.Core;
using Terminal.Gui;

namespace ConsoleFtpClient.Controllers
{
    public abstract class DataController
    {
        public List<string> Path = new List<string>()
        {
            ""
        };
        public FileStruct[] Items;
        public IClient Client;

        public abstract ListWrapper ListW();
        public abstract List<string> List();
        public abstract List<string> CreateDirection(string s);
        public abstract List<string> RemoveDirectory(string s);
        public abstract List<string> RemoveFile(string s);
        public abstract List<string> UploadFile(string s);
    }

    public interface IClient
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public FileStruct[] ListDirectory(string s);
        public void CreateDirectory(string s, string name);
        public void RemoveDirectory(string s);
        public void DeleteFile(string s);
        public void DownloadFile(string path, string fileName);
    }
}