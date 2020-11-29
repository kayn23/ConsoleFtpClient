using System;
using System.Linq;

namespace ConsoleFtpClient.Controllers
{
    public abstract class Menu
    {
        public event Action CloseMenu;

        protected void BaseSelect()
        {
            string command = "";
            string arg = "";
            var st = Console.ReadLine();
            if (st != "")
            {
                string[] parse = st.Split(" ");
                command = parse[0];
                if (parse.Length > 1)
                {
                    arg = String.Join(" ", parse.Skip(1).Take(100).ToArray());
                }
            }
            else
            {
                Console.WriteLine("Insert command");
                BaseSelect();
            }

            switch (command)
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "help":
                    RenderHelpInfo();
                    break;
                default:
                    Select(command, arg);
                    break;
            }
        }

        protected abstract void Select(string command, string arg);

        protected void Close()
        {
            CloseMenu?.Invoke();
        }

        protected virtual void RenderHelpInfo()
        {
            Console.WriteLine("help - Get help");
            Console.WriteLine("exit - Exit from programm");
            BaseSelect();
        }

        protected void RenderInfo()
        {
            // FIXME Доработать
            Console.WriteLine($"{String.Join("/", State.FtpPath)}");
        }
    }
}