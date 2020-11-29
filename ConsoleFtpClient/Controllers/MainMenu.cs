using System;
using System.Data;

namespace ConsoleFtpClient.Controllers
{
    public class MainMenu : Menu
    {
        public MainMenu()
        {
            CloseMenu += BaseSelect;
            RenderHelpInfo();
            BaseSelect();
        }

        protected override void Select(string command, string arg)
        {
            switch (command)
            {
                case "connect":
                    Console.WriteLine("Connecting...");
                    new ConnectMenu();
                    break;
                default:
                    Console.WriteLine("Undefined command");
                    Console.Clear();
                    BaseSelect();
                    break;
            }
        }

        protected override void RenderHelpInfo()
        {
            Console.WriteLine("help - Get help");
            Console.WriteLine("connect - Connect to server");
            Console.WriteLine("Exit - Close the program");
            BaseSelect();
        }
    }
}