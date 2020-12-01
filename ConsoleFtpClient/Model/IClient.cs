using System.Collections.Generic;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient.Controllers
{
    public interface IClient
    {
        public List<Item> ListDirectory(string s);
        public void CreateDirectory(string s, string name);
        public void RemoveDirectory(string s);
        public void DeleteFile(string s);
        public void UploadFile(string pathTo, string filePath);
    }
}