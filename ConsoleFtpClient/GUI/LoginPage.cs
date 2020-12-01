using System;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace ConsoleFtpClient.GUI
{
    public class LoginPage : Window
    {
        private readonly View _parent;
        public Action<string, string, string> OnLogin;
        public Action OnExit;
        
        public LoginPage(View parent) : base("Login", 5)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }
        
        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(50);
            Height = 20;
        }
        
        public void Close()
        {
            _parent?.Remove(this);
        }
        private void InitControls()
        {
            #region fields
            
            var IpLabel = new Label(0, 0, "IpAddress");
            var IpText = new TextField()
            {
                X = Pos.Left(IpLabel),
                Y = Pos.Top(IpLabel) + 1,
                Width = Dim.Fill()
            };
            IpText.Text = "127.0.0.1";
            Add(IpLabel);
            Add(IpText);

            var LoginLabel = new Label("Login")
            {
                X = Pos.Left(IpText),
                Y = Pos.Top(IpText) + 1
            };
            var LoginText = new TextField()
            {
                X = Pos.Left(IpText),
                Y = Pos.Top(LoginLabel) + 1,
                Width = Dim.Fill()
            };
            LoginText.Text = "student";
            Add(LoginLabel);
            Add(LoginText);
            
            var PasswordLable = new Label("Password")
            {
                X = Pos.Left(LoginText),
                Y = Pos.Top(LoginText) + 1
            };
            var PasswordText = new TextField()
            {
                X = Pos.Left(LoginText),
                Y = Pos.Top(PasswordLable) + 1,
                Width = Dim.Fill(),
                Secret = true
            };
            PasswordText.Text = "student";
            Add(PasswordLable);
            Add(PasswordText);
            //
            // var nameLabel = new Label(0, 0, "Nickname");
            // var nameText = new TextField("")
            // {
            //     X = Pos.Left(nameLabel),
            //     Y = Pos.Top(nameLabel) + 1,
            //     Width = Dim.Fill()
            // };
            #endregion

            #region Buttons

            var loginButton = new Button("Login", true)
            {
                X = Pos.Left(PasswordText),
                Y = Pos.Top(PasswordText) + 2
            };

            var exitButton = new Button("Exit")
            {
                X = Pos.Right(loginButton) + 5,
                Y = Pos.Top(loginButton)
            };
            Add(loginButton);
            Add(exitButton);

            exitButton.Clicked += () => OnExit?.Invoke();
            loginButton.Clicked += () =>
            {
                string IP = IpText.Text.ToString().TrimStart();
                string Login = LoginText.Text.ToString().TrimStart();
                string Password = PasswordText.Text.ToString().TrimStart();
                if (Validation(IP, Login))
                    OnLogin?.Invoke(IP, Login, Password);
            };

            #endregion
        }
        bool Validation(string ip, string login)
        {
            if (ip.Length == 0)
            {
                MessageBox.ErrorQuery(25, 5, "Error", "Ip cannot be empty.", "Ok");
                return false;
            }
            string IpPattern = @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}";
            if (!Regex.IsMatch(ip, IpPattern))
            {
                MessageBox.ErrorQuery(25, 5, "Error", "IP of the wrong format.", "Ok");
                return false;
            } 
            if (login.Length == 0)
            {
                MessageBox.ErrorQuery(25, 5, "Error", "Login cannot be empty.", "Ok");
                return false;
            }

            return true;
        }
    }
}
