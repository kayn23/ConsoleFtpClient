using System;
using ConsoleFtpClient.Controllers;
using ConsoleFtpClient.GUI;
using Terminal.Gui;

namespace ConsoleFtpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init (); 
            var top = Application.Top;
            var mainWindow = new Window("FtpClient") 
            { 
                X = 0, 
                Y = 0,
                Width = Dim.Fill (), 
                Height = Dim.Fill () 
            };
            var menu = new MenuBar (new MenuBarItem [] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () =>
                    {
                        Application.RequestStop();
                        Environment.Exit(0);
                    })
                }),
            });
            top.Add(menu);
            top.Add(mainWindow);
            LoginController.Init(mainWindow, top);
        }
    }
}
