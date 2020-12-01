using ConsoleFtpClient.Controllers;
using Terminal.Gui;

namespace ConsoleFtpClient.GUI
{
    public abstract class AListElement : Window
    {
        protected readonly View _parent;
        protected DataController _client;
        protected ListView _view;
        protected bool _isActive = true;

        protected AListElement(View parent, DataController client, string name) : base(name)
        {
            _parent = parent;
            _client = client;
            X = Pos.Left(_parent);
            Y = 0;
            Width = Dim.Percent(50);
            Height = Dim.Fill();
            Add(CreateFtpView());
        }

        private ListView CreateFtpView()
        {
            _view = new ListView
            {
                X = Pos.Left(this),
                Y = Pos.Top(this),
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1,
                Source = _client.ListW()
            };
            
            _view.OpenSelectedItem += ViewOnOpenSelectedItem;
            var mkdir = new Button("mkdir", true)
            {
                X = Pos.Left(this) + 1,
                Y = Pos.Bottom(this) - 3
            };
            var rmdir = new Button("rmdir", true)
            {
                X = Pos.Right(mkdir),
                Y = Pos.Top(mkdir)
            };
            var rm = new Button("rm", true)
            {
                X = Pos.Right(rmdir) + 1,
                Y = Pos.Top(mkdir)
            };
            var load = new Button("load", true)
            {
                X = Pos.Right(rm) + 1,
                Y = Pos.Top(mkdir)
            };
            
            mkdir.Clicked += MkdirOnClicked;
            rmdir.Clicked += RmdirOnClicked;
            rm.Clicked += RmOnClicked;
            Add(mkdir, rmdir, rm, load);
            return _view;
        }

        protected abstract void RmOnClicked();

        protected abstract void RmdirOnClicked();

        protected abstract void MkdirOnClicked();

        protected abstract void ViewOnOpenSelectedItem(ListViewItemEventArgs obj);

        protected void OperationTimeout()
        {
            _isActive = false;
            var aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += (sender, args) =>
            {
                _isActive = true;
                aTimer.Dispose();
            };
            aTimer.Start();
        }
        protected void Error(string text)
        {
            MessageBox.ErrorQuery(25, 5, "Error",
                text, "Ok");
        }
    }
}