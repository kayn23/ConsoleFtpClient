using System;
using System.Collections.Generic;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient.Controllers
{
    public abstract class DataController : DialogController
    {
        public List<string> Path = new List<string>()
        {
            ""
        };
        public List<Item> Items;
        public IClient Client;

        public abstract List<Item> List();
        protected abstract void ChangeDirectory(string s);
        protected abstract void CreateDirection(string s);
        protected abstract void RemoveDirectory(string s);
        protected abstract void RemoveFile(string s);
        protected abstract void UploadFile(string s);

        protected override void RenderList()
        {
            foreach (var item in List())
            {
                Console.WriteLine($"{item.Name}");
            }
        }
    }
}