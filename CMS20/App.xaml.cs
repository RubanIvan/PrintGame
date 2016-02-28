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
        private GameTitleDir game;
        private GameContent content;
        private LogText log;
        private ParseUrl url;
        private Test test;
        private ImageCreate image;
        private TagManage tag;
        private FileManage file;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            window=new MainWindow();
            log=new LogText(window);
            url=new ParseUrl(window);
            game=new GameTitleDir(window);
            content=new GameContent(window);
            test=new Test(window);
            window.Test = test;
            image=new ImageCreate(window);
            tag=new TagManage(window);
            file=new FileManage(window);

            window.Show();
        }

    }
}
