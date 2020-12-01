using System.Linq;
using ConsoleFtpClient.Core;
using ConsoleFtpClient.GUI;
using Terminal.Gui;

namespace ConsoleFtpClient.Controllers
{
    public class MainWindowController
    {
        private readonly View _top;
        private readonly DataController _ftpClient;
        private readonly DataController _localClient;

        public MainWindowController(View top, DataController ftpClient, DataController localClient)
        {
            _top = top;
            _ftpClient = ftpClient;
            _localClient = localClient;
            InitWindow();
        }

        private void InitWindow()
        {
            var w1 = new FtpElement(_top, _ftpClient);
            var w2 = new Window()
            {
                X = Pos.Right(w1), 
                Y = 0, // Оставляем одну строку для меню верхнего уровня
                // Используя Dim.Fill (), он автоматически изменит размер без ручного вмешательства 
                Width = Dim.Percent(50), 
                Height = Dim.Fill () - 1
            };
            _top.Add(w1);
            _top.Add(w2);
        }
    }
}