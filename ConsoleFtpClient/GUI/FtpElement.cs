using System;
using System.IO;
using System.Linq;
using ConsoleFtpClient.Controllers;
using Terminal.Gui;

namespace ConsoleFtpClient.GUI
{
    public class FtpElement : AListElement
    {
        public FtpElement(View parent, DataController client)
            : base(parent, client, "Ftp server")
        {}

        protected override void RmOnClicked()
        {
            if (_isActive)
            {
                var index = _view.SelectedItem;
                var item = _client.Items[index];
                if (!item.IsDirectory)
                {
                    _view.SetSource(_client.RemoveFile(item.Name));
                }
                else
                {
                    Error("Not is File");
                }

                var l = _client.List();
                _view.SetSource(l);
                OperationTimeout();
            }
        }

        protected override void RmdirOnClicked()
        {
            if (_isActive)
            {
                var index = _view.SelectedItem;
                var item = _client.Items[index];
                if (item.IsDirectory)
                {
                    _view.SetSource(_client.RemoveDirectory(item.Name));
                }
                else
                {
                    Error("Not a directory");
                }
                OperationTimeout();
            }
        }

        protected override void MkdirOnClicked()
        {
            if (_isActive)
            {
                if (_isActive)
                {
                    var ok = new Button("Ok");
                    var cancel = new Button("Cancel");
                    var input = new TextField()
                    {
                        X = 0,
                        Y = Pos.Center(),
                        Width = Dim.Fill(),
                    };
                    var a = new Dialog("Create Directory", 25, 4, ok, cancel);
                    cancel.Clicked += () => _parent.Remove(a);
                    ok.Clicked += () =>
                    {
                        var s = input.Text;
                        if (_client.Items.Count(i => i.Name == s) == 0)
                        {
                            var l = _client.CreateDirection(s.ToString());
                            _view.SetSource(l);
                            Remove(a);
                        }
                        else
                        {
                            Error("Directory exists");
                        }
                    };
                    a.Add(input);
                    Add(a);
                    input.SetFocus();
                    _isActive = false;
                }
                OperationTimeout();
            }
        }

        protected override void ViewOnOpenSelectedItem(ListViewItemEventArgs obj)
        {
            if (_isActive)
            {
                var listIndex = _view.SelectedItem;
                if (listIndex == 0)
                {
                    if (_client.Path.Count == 1)
                    {
                        _view.SetSource(_client.List());
                    }
                    else
                    {
                        _client.Path.Remove(_client.Path.Last());
                        _view.SetSource(_client.List());
                    }
                }
                else
                {
                    var items = _client.Items;
                    var f = items[listIndex];
                    if (f.IsDirectory)
                    {
                        _client.Path.Add(f.Name);
                        _view.SetSource(_client.List());
                    }
                }
                
                OperationTimeout();
            }
        }
    }
}