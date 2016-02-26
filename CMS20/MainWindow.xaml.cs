﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMS20
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>каталог КУДА будут скопированы файлы</summary>
        public string DstGameDirPath = @"D:\Content\GameData\";

        /// <summary>каталог ОТ КУДА будут скопированы файлы</summary>
        public string SrcGameDirPath = @"D:\Game_Base";

        /// <summary>каталог с архивами</summary>
        public string FileShareFolder = @"D:\Content\FileShare\";  

        /// <summary>ID текущей игры</summary>
        public int GameID;

        /// <summary>список игр которые мы хотим добавить в базу</summary>
        public List<string> GameList = new List<string>();
        public int GameListIndex = 0;


    }
}
