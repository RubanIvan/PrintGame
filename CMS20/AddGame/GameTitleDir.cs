using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMS20.Controllers
{
    public class GameTitleDir
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        public GameTitleDir(MainWindow w)
        {
            W = w;
            W.ButtonSetDir.Click += ButtonSetDir_Click;
            W.ButtonPrev.Click += ButtonPrev_Click;
            W.ButtonNext.Click += ButtonNext_Click;
            W.ButtonCopyGameName.Click += ButtonCopyGameName_Click;
            
            SetDir();

         }

        #region DirSetup

        /// <summary>Установка начальных каталогов</summary>
        private void SetDir()
        {
            W.LabelSetDir.Content = W.SrcGameDirPath;
            W.GameList = Directory.GetDirectories(W.SrcGameDirPath).ToList();
            //Проверяем если каталог скрыт не отображаем его
            for (int i = 0; i < W.GameList.Count; i++)
            {
                DirectoryInfo d=new DirectoryInfo(W.GameList[i]);
                if((d.Attributes & FileAttributes.Hidden) != 0)W.GameList.RemoveAt(i);
            }
            ShowGameListIndex();
        }

        //отобразить индекс текущей игры 
        private void ShowGameListIndex()
        {
            W.LabelGameListIndex.Content = W.GameListIndex + " / " + W.GameList.Count;
            W.LabelCatName.Content = W.GameList[W.GameListIndex];
        }

        #endregion

        /// <summary>Перейти к предыдущей игре </summary>
        private void ButtonPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            W.GameListIndex--;
            if (W.GameListIndex < 0) W.GameListIndex = W.GameList.Count - 1;
            ShowGameListIndex();
        }

        /// <summary>Перейти к следующей игре</summary>
        private void ButtonNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            W.GameListIndex++;
            if (W.GameListIndex >= W.GameList.Count) W.GameListIndex = 0;
            ShowGameListIndex();
        }

        /// <summary>Скопировать название на основе каталога с игрой создать выходной каталог</summary>
        private void ButtonCopyGameName_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Копировать название из каталога
            string dirname = System.IO.Path.GetFileName(W.GameList[W.GameListIndex]);
            W.TextBoxTitleEn.Text = dirname;
            W.TextBoxCatalogName.Text = dirname;

            //Создание директории для картинок 
            try
            {
                Directory.CreateDirectory(W.DstGameDirPath + W.TextBoxCatalogName.Text);
            }
            catch (Exception ee)
            {
                W.Log.Add("Ошибка создания дериктории\n" + ee.Message);
                return;
            }

            W.Log.Add("Директория создана: " + W.DstGameDirPath + W.TextBoxCatalogName.Text);

        }


        /// <summary>Устанавливаем каталог от куда будем брать игры</summary>
        private void ButtonSetDir_Click(object sender, System.Windows.RoutedEventArgs e) { }
    }
}
