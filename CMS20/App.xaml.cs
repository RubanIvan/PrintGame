using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CMS20.Controllers;
using CMS20.FileShareManage;

namespace CMS20
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;
        private MainMenu menu;
        private GameTitleDir game;
        private GameContent content;
        private LogText log;
        private ParseUrl url;
        private Test test;
        private ImageCreate image;
        private TagManage tag;
        private FileManage file;
        private FileManageGUI fileGui;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            window=new MainWindow();
            menu=new MainMenu(window);
            log=new LogText(window);
            url=new ParseUrl(window);
            game=new GameTitleDir(window);
            content=new GameContent(window);
            test=new Test(window);
            window.Test = test;
            image=new ImageCreate(window);
            tag=new TagManage(window);
            file=new FileManage(window);
            fileGui=new FileManageGUI(window);

            window.Show();
        }

    }
}
