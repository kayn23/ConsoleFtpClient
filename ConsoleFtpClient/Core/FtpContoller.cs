using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConsoleFtpClient.Controllers;
using Terminal.Gui;

namespace ConsoleFtpClient.Core
{
    public class FileController : DataController
    {
        public FileController(IClient client)
        {
            Client = client;
        }
        public override ListWrapper ListW()
        {
            return new ListWrapper(List());
        }
        public override List<string> List()
        {
            string path = String.Join("/", Path);
            var list = Client.ListDirectory(path + "/");
            var a = new List<string>();
            Items = list;
            a.AddRange(list.Select((i) => i.Name).ToList());
            return a;
        }

        public override List<string> CreateDirection(string s)
        {
            try
            {
                Client.CreateDirectory(
                    String.Join("/", Path) + "/",
                    s.ToString()
                );
            }
            catch (Exception e)
            {
                MessageBox.ErrorQuery(25, 5, "Error", e.Message, "Ok");
            }
            return List();
        }
        public override List<string> RemoveDirectory(string s)
        {
            try
            {
                var path = String.Join("/", Path) + "/" + s;
                Client.RemoveDirectory(path);
            }
            catch (WebException e)
            {
                var a = ((FtpWebResponse) e.Response).StatusDescription;
                MessageBox.ErrorQuery(25, 5, "Error", a.ToString(), "Ok");
            }
            return List();
        }
        public override List<string> RemoveFile(string s)
        {
            try {
                Client.DeleteFile(String.Join("/", Path) + "/" + s);
            }
            catch (WebException e)
            {
                var a = ((FtpWebResponse) e.Response).StatusDescription;
                MessageBox.ErrorQuery(25, 5, "Error", a.ToString(), "Ok");
            }
            return List();
        }

        public override List<string> UploadFile(string s)
        {
            return List();
        }
    }
}