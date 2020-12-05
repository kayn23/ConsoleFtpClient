using System;
using System.IO;
using System.Linq;
using ConsoleFtpClient.Controllers;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.Model;

namespace ConsoleFtpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var a  = new LoginController();
            a.LoginSuccess += AOnLoginSuccess;
            a.ReadData();
        }

        private static void AOnLoginSuccess((DataController, DataController) obj)
        {
            State.FtpController = obj.Item1;
            obj.Item1.Exit += () => Environment.Exit(0);
            obj.Item2.Exit += () => Environment.Exit(0);
            State.FtpController = obj.Item1;
            State.LocalController = obj.Item2;
            obj.Item1.WaitInput();
        }
    }
}
