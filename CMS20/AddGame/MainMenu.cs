using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CMS20.Controllers
{

    class MainMenu
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        public MainMenu(MainWindow w )
        {
            W = w;
            W.ButtonAddGame.Click += ButtonAddGame_Click;
            w.ButtonDeposit.Click += ButtonDeposit_Click;

        }

        private void ButtonAddGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            W.GridMainMenu.Visibility=Visibility.Hidden;
            W.GridAddGame.Visibility=Visibility.Visible;
        }

        private void ButtonDeposit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            W.GridMainMenu.Visibility = Visibility.Hidden;
            W.GridDepositFileManage.Visibility = Visibility.Visible;
        }

    }
}
