using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CMS20.Controllers;

namespace CMS20
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;
        private Game game;
        private LogText log;
        private ParseUrl url;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            window=new MainWindow();
            log=new LogText(window);
            url=new ParseUrl(window);
            game=new Game(window);
            window.Show();
        }

    }
}
