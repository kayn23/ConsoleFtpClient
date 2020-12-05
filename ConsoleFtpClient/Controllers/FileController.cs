using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.Service;

namespace ConsoleFtpClient.Controllers
{
    public class FileController : DataController
    {
        public FileController(IClient client, WorkDir wd)
        {
            _wd = wd;
            Client = client;
        }
        
        protected override void Selector(string command, string arg)
        {
            switch (command)
            {
                case "ls":
                    WaitInput();
                    break;
                case "cd":
                    ChangeDirectory(arg);
                    WaitInput();
                    break;
                case "mkdir":
                    CreateDirection(arg);
                    WaitInput();
                    break;
                case "rm":
                    RemoveDirectory(arg);
                    WaitInput();
                    break;
                case "cp":
                    UploadFile(arg);
                    WaitInput();
                    break;
                case "cw":
                    SwitchDisplay();
                    break;;
                case "help":
                    HelpRender();
                    WaitInput();
                    break;
                default:
                    WaitInput();
                    break;
            }
        }

        private void HelpRender()
        {
            Console.WriteLine("ls - Обновить представление");
            Console.WriteLine("cd <dirname> - перейти в директорию");
            Console.WriteLine("mkdir <dirname> - Создать директироию");
            Console.WriteLine("rm <name> - Удалить директорию/файл");
            Console.WriteLine("cp <name> - Загрузить файл");
            Console.WriteLine("cw - сменить представление");
            Console.ReadLine();
        }

        private void SwitchDisplay()
        {
            switch (_wd)
            {
                case WorkDir.Local:
                    Console.Clear();
                    State.FtpController.WaitInput();
                    break;;
                case WorkDir.Server:
                    Console.Clear();
                    State.LocalController.WaitInput();
                    break;
            }
        }

        public override List<Item> List()
        {
            string path = String.Join("/", Path);
            var a = Client.ListDirectory(path + "/");
            Items = a;
            return Items;
        }

        protected override void ChangeDirectory(string s)
        {
            if (s == "..")
            {
                if (Path.Count > 1)
                    Path.Remove(Path.Last());
            }
            else
            {
                var item = Items.FirstOrDefault(i => i.Name == s);
                if (item.FullLink == null)
                {
                    ConsoleService.Error("Нет такой директории");
                    WaitInput();
                }
                else if(item.IsDirectory)
                {
                    Path.Add(s);
                }
                else
                {
                    ConsoleService.Error("Это файл");
                }
            }
        }

        protected override void CreateDirection(string s)
        {
            try
            {
                Client.CreateDirectory(
                    String.Join("/", Path) + "/",
                    s.ToString()
                );
            }
            catch (WebException e)
            {
                var a = ((FtpWebResponse) e.Response).StatusDescription;
                ConsoleService.Error(a);
            }
        }

        protected override void RemoveDirectory(string s)
        {
            var item = Items.FirstOrDefault(i => i.Name == s);
            if (s == ".." || item.FullLink == "")
            {
                ConsoleService.Error("Нельзя удалить");
                return;
            }

            if (item.IsDirectory)
            {
                try
                {
                    var path = String.Join("/", Path) + "/" + s;
                    Client.RemoveDirectory(path);
                }
                catch (WebException e)
                {
                    var a = ((FtpWebResponse) e.Response).StatusDescription;
                    ConsoleService.Error(a);
                }
            }
            else
            {
                RemoveFile(s);
            }
        }

        protected override void RemoveFile(string s)
        {
            try {
                Client.DeleteFile(String.Join("/", Path) + "/" + s);
            }
            catch (WebException e)
            {
                var a = ((FtpWebResponse) e.Response).StatusDescription;
                ConsoleService.Error(a);
            }
        }

        protected override void UploadFile(string s)
        {
            if (Items.Count(i => i.Name == s) == 0)
            {
                ConsoleService.Error("Нет такого файла");
                Console.Read();
            }
            var item = Items.FirstOrDefault(i => i.Name == s);
            if (item.IsDirectory)
            {
                // TODO РЕализация
                ConsoleService.Error("Копирование директорий не реализованно");
                return;
            }
            var pathFrom = String.Join("/", Path) + "/" + s;
            string pathTo = "/";

            if (_wd == WorkDir.Local)
            {
                pathTo = String.Join("/", State.FtpController.Path);
            }

            if (_wd == WorkDir.Server)
            {
                pathTo = String.Join("/", State.LocalController.Path);
            }
            
            Client.UploadFile(pathTo, pathFrom);
        }
    }
}