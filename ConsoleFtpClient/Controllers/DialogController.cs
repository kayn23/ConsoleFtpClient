using System;
using System.Text.RegularExpressions;
using ConsoleFtpClient.Core;

namespace ConsoleFtpClient.Controllers
{
    public abstract class DialogController
    {
        public event Action Exit;
        protected WorkDir _wd;
        protected abstract void Selector(string command, string arg);

        public void WaitInput()
        {
            Render();
            Console.Write("> ");
            string s = "";
            while (s.Length == 0)
            {
                s = Console.ReadLine();
            }
            var a = parseInput(s);
            if (a.Item1 == "exit")
            {
                Exit();
                return;
            }

            if (a.Item1 == "clear")
            {
                Console.Clear();
            }
            Selector(a.Item1, a.Item2);
        }

        private (string, string) parseInput(string s)
        {
            var comPattern = @"^[a-z]+";
            var argPattern = @"(?<=[ ]).+$";
            var comand = Regex.Match(s, comPattern).Value;
            var arg = Regex.Match(s, argPattern).Value;
            return (comand, arg);
        }

        private void Render()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Active: ");
            switch (_wd)
            {
                case WorkDir.Local:
                    Console.WriteLine("LocalMachine");
                    break;;
                case WorkDir.Server:
                    Console.WriteLine("FtpServer");
                    break;
            }
            Console.ResetColor();
            if (State.FtpController != null)
            {
                var path = String.Join("/", State.FtpController.Path);
                path += "/";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("ftp_path: ");
                Console.ResetColor();
                Console.WriteLine(path);
            }
            if (State.LocalController != null)
            {
                var path = String.Join("/", State.LocalController.Path);
                path += "/";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("local_path: ");
                Console.ResetColor();
                Console.WriteLine(path);
            }
            RenderList();
        }

        protected abstract void RenderList();
    }
}