using System;
using System.Runtime.CompilerServices;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.GUI;
using Terminal.Gui;

namespace ConsoleFtpClient.Controllers
{
    public class LoginController
    {
        private readonly View _parent;
        private readonly LoginPage _login;
        private readonly Toplevel _top;
        private LoginController(View parent, Toplevel top)
        {
            _parent = parent;
            _top = top;
            _login = new LoginPage(parent);
            // _login.OnExit += () => Application.RequestStop();
            _login.OnLogin += Login;
            _login.OnExit += _login.Close;
            _parent.Add(_login);
            Application.Run((Toplevel)_parent);
        }
        public void Login(string ip, string login, string pass)
        {
            DataController ftpClient = new FileController(new FtpClient
                {
                    Host = ip,
                    UserName = login,
                    Password = pass
                });
            try
            {
                var a = ftpClient.List();
                _login.Close();
                // TODO Инициализация локального клиента тут
                new MainWindowController(_parent, ftpClient, ftpClient);
                Application.Run(_top);
            }
            catch
            {
                MessageBox.ErrorQuery(25, 5, "Error", "Incorrect connection data.", "Ok");
            }
        }

        public static void Init(View parent, Toplevel top)
        {
            new LoginController(parent, top);
        }
    }
}