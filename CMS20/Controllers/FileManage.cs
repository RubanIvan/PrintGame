using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CMS20.Properties;

namespace CMS20.Controllers
{
    class FileManage
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        /// <summary>Путь к WinRar</summary>
        private static string RarPath= Settings.Default.RarPath;

        private static string ZipDescFile = Settings.Default.ZipDescFile;

        List<Label> LabelList = new List<Label>();
        List<TextBox> TextBoxList = new List<TextBox>();

        List<string> ListFileShare = new List<string>();
        List<string> ArcName = new List<string>();

        public FileManage(MainWindow w)
        {   
            W = w;

            LabelList.Add(W.LabelFailName0); LabelList.Add(W.LabelFailName1); LabelList.Add(W.LabelFailName2);
            LabelList.Add(W.LabelFailName3); LabelList.Add(W.LabelFailName4); LabelList.Add(W.LabelFailName5);
            TextBoxList.Add(W.TextBoxFailNameDesc0); TextBoxList.Add(W.TextBoxFailNameDesc1); TextBoxList.Add(W.TextBoxFailNameDesc2);
            TextBoxList.Add(W.TextBoxFailNameDesc3); TextBoxList.Add(W.TextBoxFailNameDesc4); TextBoxList.Add(W.TextBoxFailNameDesc5);

            W.ButtonFileShareScan.Click += ButtonFileShareScan_Click;
            W.ButtonZip.Click += ButtonZip_Click;
            W.ButtonArcToSql.Click += ButtonArcToSql_Click;

        }

        //сканируем директорию
        private void ButtonFileShareScan_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //очищаем списки
            foreach (Label label in LabelList) label.Content = "";
            foreach (TextBox textBox in TextBoxList) textBox.Text = "";

            //сканируем директорию ищем подкаталоги
            //каждый подкаталог надо зделать архивом
            ListFileShare = Directory.GetDirectories(Path.Combine(W.SrcGameDirPath, W.GameCatalogName)).ToList();

            for (int i = 0; i < ListFileShare.Count; i++)
            {
                ListFileShare[i] += "\\";
                LabelList[i].Content = ListFileShare[i];
                ArcName.Add(Path.Combine(W.FileShareFolder, W.GameCatalogName) + $"{i}.zip");
                TextBoxList[i].Text = W.GameCatalogName;
            }

        }

        //Создаем из найденных каталогов архивы
        private void ButtonZip_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string rarcmd = RarPath + $" a -r -m3 -ep1 -z{ZipDescFile} -ms*.rar;*.zip;*.jpg ";
            for (int i = 0; i < ListFileShare.Count; i++)
            {
                Process.Start("cmd.exe", " /K " + rarcmd + "\"" + ArcName[i] + "\" " + "\"" + LabelList[i].Content + "\"");
            }
        }

        //записываем данные о файлах в базу
        private void ButtonArcToSql_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();



            for (int i = 0; i < ArcName.Count; i++)
            {
                FileShare fs = new FileShare();
                fs.GameID = W.GameID;
                fs.FileShareName = ArcName[i].Substring(W.FileShareFolder.Length);
                fs.FileShareDesc = TextBoxList[i].Text;

                FileInfo file = new System.IO.FileInfo(ArcName[i]);
                long size = file.Length;
                //(Math.Floor(Math.Log(bytes, 1024)))
                double f = size / (1024.0 * 1024.0);
                fs.FileShareSize = f.ToString("F") + "MB";

                GameEntities.FileShare.Add(fs);
            }

            try
            {
                GameEntities.SaveChanges();
                W.Log.Add($"В базу добавленно {ArcName.Count} записи о файлах");

                DirectoryInfo dir=new DirectoryInfo(Path.Combine(W.SrcGameDirPath, W.GameCatalogName));
                dir.Attributes=FileAttributes.Hidden;


                W.GridFileManage.Visibility=Visibility.Hidden;
                ClearAllData();
                W.GridCreateGame.Visibility=Visibility.Visible;

                
            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
        }

        //Очищаем данные
        private void ClearAllData()
        {
            W.TextBoxTitleRu.Clear();
            W.TextBoxTitleEn.Clear();
            W.TextBoxCatalogName.Clear();
            W.TextBoxYearPublishing.Clear();
            W.TextBoxRating.Clear();
            W.TextBoxNumOfPlayers.Clear();
            W.TextBoxNumOfSuggested.Clear();
            W.TextBoxSuggestedAges.Clear();
            W.TextBoxAcquaintance.Clear();
            W.TextBoxPlayingTime.Clear();
            W.TextBoxDescript.Clear();
            W.TextBoxComponents.Clear();

            W.Image0.Source = W.DefImg;
            W.Image1.Source = W.DefImg;
            W.Image2.Source = W.DefImg;
            W.Image3.Source = W.DefImg;
            W.Image4.Source = W.DefImg;
            W.Image5.Source = W.DefImg;
            W.Image6.Source = W.DefImg;
            W.Image7.Source = W.DefImg;
            W.Image8.Source = W.DefImg;
            W.Image9.Source = W.DefImg;

            W.TextBoxImage0.Clear();
            W.TextBoxImage1.Clear();
            W.TextBoxImage2.Clear();
            W.TextBoxImage3.Clear();
            W.TextBoxImage4.Clear();
            W.TextBoxImage5.Clear();
            W.TextBoxImage6.Clear();
            W.TextBoxImage7.Clear();
            W.TextBoxImage8.Clear();
            W.TextBoxImage9.Clear();

            W.ListBoxGameTag.Items.Clear();

            W.LabelFailName0.Content = "";
            W.LabelFailName1.Content = "";
            W.LabelFailName2.Content = "";
            W.LabelFailName3.Content = "";
            W.LabelFailName4.Content = "";
            W.LabelFailName5.Content = "";

            W.TextBoxFailNameDesc0.Clear();
            W.TextBoxFailNameDesc1.Clear();
            W.TextBoxFailNameDesc2.Clear();
            W.TextBoxFailNameDesc3.Clear();
            W.TextBoxFailNameDesc4.Clear();
            W.TextBoxFailNameDesc5.Clear();
        }

    }
}
