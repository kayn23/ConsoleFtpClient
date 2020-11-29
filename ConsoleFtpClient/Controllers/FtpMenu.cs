using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient.Controllers
{
    public class FtpMenu : Menu
    {
        public FtpMenu()
        {
            ListShow();
        }
        protected override void Select(string command, string arg)
        {
            switch (command)
            {
                case "ls":
                    ListShow();
                    break;
                case "cd":
                    Console.WriteLine(arg);
                    ChangeDirectory(arg);
                    break;
                case "mkdir":
                    CreateDirectory(arg);
                    break;
                case "rmdir":
                    RemoveDirectory(arg);
                    break;
                case "rm":
                    RemoveFile(arg);
                    break;
                default:
                    Console.WriteLine("Undefined command");
                    Console.Clear();
                    BaseSelect();
                    break;
            }
        }


        // FIXME Настройить прием значение с пробелами
        protected override void RenderHelpInfo()
        {
            Console.WriteLine("local - Switch to local machine");
            Console.WriteLine("ls - Show list of files and directories");
            Console.WriteLine("cd <directory name> - go to directory");
            Console.WriteLine("mkdir <dirname> - Create directory");
            Console.WriteLine("rmdir <dirname> - Delete directory");
            Console.WriteLine("load <file name> - Download file");
            Console.WriteLine("rm <filename> - Delete file");
            base.RenderHelpInfo();
        }

        #region Client command
        private void ListShow()
        {
            Console.Clear();
            RenderInfo();
            string path = String.Join("/", State.FtpPath);
            var list = State.Client.ListDirectory(path + "/");
            State.FtpItems = list;
            RenderList(list);
            BaseSelect();
        }
        
        private void ChangeDirectory(string s)
        {
            if (s == "..")
            {
                State.FtpPath.Remove(State.FtpPath.Last());
                ListShow();
            }
            if (State.FtpItems.Where((i) => i.Name == s && i.IsDirectory).Count() == 1)
            {
                State.FtpPath.Add(s);
                ListShow();
            }
            else
            {
                Console.WriteLine("Incorrect value");
                BaseSelect();
            }
        }
        
        private void CreateDirectory(string s)
        {
            State.Client.CreateDirectory(
                String.Join("/", State.FtpPath) + "/",
                s
                );
            ListShow();
        }
        
        private void RemoveDirectory(string s)
        {
                State.Client.RemoveDirectory(
                    String.Join("/", State.FtpPath) + "/" + s
                );
                ListShow();
        }
        private void RemoveFile(string s)
        {
            State.Client.DeleteFile(String.Join("/", State.FtpPath) + "/" + s);
            ListShow();
        }

        #endregion

        void RenderList(FileStruct[] items)
        {
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name} {item.Flags} {item.CreateTime}");
            }
        }
    }
}