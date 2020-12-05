using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ConsoleFtpClient.Controllers;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient.Model
{
    public class LocalClient : IClient
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public LocalClient()
        {
        }
        public List<Item> ListDirectory(string s)
        {
            s = s.Replace('/', '\\');
            List<Item> items = new List<Item>();
            items.Add(new Item()
            {
                Name = "..",
                FullLink = "..",
                IsDirectory = true
            });
            var directories = Directory.GetDirectories(s);
            foreach (var directory in directories)
            {
                items.Add(Parse(directory, true));
            }
            var files = Directory.GetFiles(s);
            foreach (var file in files)
            {
                items.Add(Parse(file));
            }

            return items;
        }

        public void CreateDirectory(string s, string name)
        {
            s = s.Replace('/', '\\');
            Directory.CreateDirectory(s + "\\" + name);
        }

        public void RemoveDirectory(string s)
        {
            s = s.Replace('/', '\\');
            Directory.Delete(s);
        }

        public void DeleteFile(string s)
        {
            s = s.Replace('/', '\\');
            File.Delete(s);
        }

        public void UploadFile(string path, string fileName)
        {
            FtpWebRequest ftpRequest;
            fileName = fileName.Replace('/', '\\');
            string shortName = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);

            FileStream uploadedFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            ftpRequest = (FtpWebRequest) WebRequest.Create("ftp://" + Host + path + "/" + shortName);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.EnableSsl = UseSsl;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] file_to_bytes = new byte[uploadedFile.Length];
            uploadedFile.Read(file_to_bytes, 0, file_to_bytes.Length);
            uploadedFile.Close();

            Stream writer = ftpRequest.GetRequestStream();

            writer.Write(file_to_bytes, 0, file_to_bytes.Length);
            writer.Close();
        }

        private Item Parse(string s, bool dir = false)
        {
            s = s.Replace('/', '\\');
            var item = new Item();
            var sParser = s.Split("\\");
            item.Name = sParser.Last();
            item.FullLink = s;
            item.IsDirectory = dir;
            return item;
        }
    }
}