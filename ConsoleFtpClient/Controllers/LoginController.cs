using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.Model;
using ConsoleFtpClient.Service;

namespace ConsoleFtpClient.Controllers
{
    public class LoginController
    {
        public event Action<(DataController, DataController)> LoginSuccess;
        void Login(string ip, string login, string pass)
        {
            DataController ftpClient = new FileController(new FtpClient
                {
                    Host = ip,
                    UserName = login,
                    Password = pass
            }, WorkDir.Server);
            try
            {
                var a = ftpClient.List();
                DataController localClient = new FileController(new LocalClient()
                {
                    Host = ip,
                    UserName = login,
                    Password = pass
                }, WorkDir.Local);
                localClient.Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    .Split("\\").ToList();
                Console.Clear();
                LoginSuccess((ftpClient, localClient));
            }
            catch
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка при подключение. Проверьте введеные данные.");
                Console.ResetColor();
                ReadData();
            }
        }

        public void ReadData()
        {
            #if !DEBUG
                Console.Write("Введите адресс сервера: ");
                var ip = Console.ReadLine();
                Console.Write("Введите логин: ");
                var login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                var password = ConsoleService.GetPassword();
            #else
                var ip = "127.0.0.1";
                var login = "student";
                var password = "student";
            #endif

            if (Validation(ip, login))
            {
                Login(ip, login, password);
            }
            else
            {
                Console.Clear();
                ReadData();
            }
        }

        private bool Validation(string ip, string login)
        {
            if (ip.Length == 0)
            {
                ConsoleService.Error("Ip cannot be empty.");
                return false;
            }
            // string IpPattern = @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}";
            // if (!Regex.IsMatch(ip, IpPattern))
            // {
            //     ConsoleService.Error("IP of the wrong format.");
            //     return false;
            // } 
            if (login.Length == 0)
            {
                ConsoleService.Error("Login cannot be empty.");
                return false;
            }

            return true;
        }


        public static void Init()
        {
            new LoginController();
        }
    }
}