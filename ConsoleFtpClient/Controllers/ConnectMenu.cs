using System;
using System.Text;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.Service;

namespace ConsoleFtpClient.Controllers
{
    public class ConnectMenu : Menu
    {
        private string IP;
        private int PORT;
        private string UserName;
        private string Password;
        public ConnectMenu()
        {
            InsertData();
        }
        protected override void Select(string command, string arg)
        {
            throw new NotImplementedException();
        }

        // TODO Настроить валидации
        void InsertIP()
        {
            Console.Write("Instert host IP address: ");
            IP = Console.ReadLine();
        }

        void InsertPort()
        {
            Console.Write("Insert host Port: ");
            string port = Console.ReadLine();
            if (port == "") InsertPort();
            try
            {
                PORT = Int32.Parse(port);
            }
            catch
            {
                InsertPort();
            }
        }

        void InsertName()
        {
            Console.Write("Insert UserName: ");
            UserName = Console.ReadLine();
        }

        void InsertPassword()
        {
            Console.Write("Insert Password: ");
            Password = ConsoleService.GetPassword();
        }

        void InsertData()
        {
            InsertIP();
            InsertName();
            InsertPassword();
            State.Client = new FtpClient
            {
                Host = IP,
                UserName = UserName,
                Password = Password
            };
            try
            {
                var a = State.Client.ListDirectory(null);
                new FtpMenu();
            }
            catch
            {
                Console.WriteLine("Incorrect data entered");
                InsertData();
            }
        }
    }
}