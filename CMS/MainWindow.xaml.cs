using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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


namespace CMS
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {

        string DstGameDirPath = @"D:\Content\GameData\";   //каталог куда будут скопированы файлы
        string SrcGameDirPath = @"D:\Game_Base";            //каталог от куда будут копировать
        private string GameCatalogName;                            //каталог c игрой
        int GameID ; //ID вставленной записи
        List<string> GameList = new List<string>();
        private int GameListIndex = 0;

        List<string> ImageListJpg = new List<string>();        //изображения загруженные из каталога
        List<string> ImageListPng = new List<string>();        //изображения загруженные из каталога
        List<Image> WinImage = new List<Image>();           //изображения на окне

        List<Tag> TagList=new List<Tag>();                  //список тэгов 

        public MainWindow()
        {
            InitializeComponent();
            SetDir();
        }

        #region Установки каталогов

        private void SetDir()
        {
            LabelSetDir.Content = SrcGameDirPath;
            GameList = Directory.GetDirectories(SrcGameDirPath).ToList();
            ShowGameListIndex();
        }

        private void ButtonSetDir_Click(object sender, RoutedEventArgs e)
        { }


        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            GameListIndex--;
            if (GameListIndex < 0) GameListIndex = GameList.Count - 1;
            ShowGameListIndex();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            GameListIndex++;
            if (GameListIndex >= GameList.Count) GameListIndex = 0;
            ShowGameListIndex();
        }

        private void ShowGameListIndex()
        {
            LabelGameListIndex.Content = GameListIndex + " / " + GameList.Count;
            LabelCatName.Content = GameList[GameListIndex];
        }
        #endregion


        #region первый экран (создание записи об игре)

        //Копировать название из каталога
        private void ButtonCopyGameName_Click(object sender, RoutedEventArgs e)
        {
            string dirname = System.IO.Path.GetFileName(GameList[GameListIndex]);
            TextBoxTitleEn.Text = dirname;
            TextBoxCatalogName.Text = dirname;
        }

        //Создание директории для картинок 
        private void ButtonCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCatalogName.Text == "")
            {
                TextBoxLogWindow.Text += "Неверно указано имя дериктории\n";
                return;
            }

            try
            {
                Directory.CreateDirectory(DstGameDirPath + "\\" + TextBoxCatalogName.Text);
            }
            catch (Exception ee)
            {
                TextBoxLogWindow.Text += "Ошибка создания дериктории\n" + ee.Message + "\n";
                return;
            }

            TextBoxLogWindow.Text += "Директория создана\n" + TextBoxCatalogName.Text + "\n";




        }

        //Обрамление строк в параграф
        private void ButtonTagP_Click(object sender, RoutedEventArgs e)
        {
            //Если не нашли открывающего тега
            if (TextBoxDescript.Text.Contains("<p>") == false)
            {
                //то обрамляем строки в тег <p>..</p>
                string[] TextList = TextBoxDescript.Text.Split(new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                TextBoxDescript.Text = "";
                foreach (string t in TextList)
                {
                    TextBoxDescript.Text += "<p>" + t + "</p>\n";
                }

            }
        }

        //Создание списка
        private void ButtonTagLi_Click(object sender, RoutedEventArgs e)
        {
            //Если не нашли открывающего тега
            if (TextBoxComponents.Text.Contains("<li>") == false)
            {
                //то обрамляем строки в тег <li>..</li>
                string[] TextList = TextBoxComponents.Text.Split(new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                TextBoxComponents.Text = "";
                foreach (string t in TextList)
                {
                    TextBoxComponents.Text += "<li>" + t + "</li>\n";
                }

            }
        }

        //Проверка ввели ли правельно год
        private void TextBoxYearPublishing_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxYearPublishing.Text == "") return;
            int year = 0;
            //Пробуем перевести в число
            if (int.TryParse(TextBoxYearPublishing.Text, out year) == false)
            {
                //перевод не удался стираем неверное значение
                TextBoxYearPublishing.Text = "";
                return;
            }
            //если указан короткий год то дополняем его
            if (year >= 70 && year <= 99) TextBoxYearPublishing.Text = (1900 + year).ToString();
            if (year >= 0 && year <= 20) TextBoxYearPublishing.Text = (2000 + year).ToString();
        }

        //Тест коректно ли введен год
        private bool TestYearPublishing()
        {
            int year = 0;
            if (int.TryParse(TextBoxYearPublishing.Text, out year) == false)
            {
                TextBoxLogWindow.Text += "Неверно введен год\n";
                return false;
            }
            //проверка попадания в диапозон
            if (year < 1970 || year > 2020)
            {
                TextBoxLogWindow.Text += "Неверно введен год ошибка диапозона\n";
                return false;
            }
            return true;
        }

        //Проверка на ввод рейтинга
        private void TextBoxRating_LostFocus(object sender, RoutedEventArgs e)
        {
            float ratio;
            if (float.TryParse(TextBoxRating.Text, out ratio) == false)
            {
                TextBoxLogWindow.Text += "Неверно введен рейтинг\n";
                return;
            }
            if (ratio < 0 || ratio > 10)
            {
                TextBoxLogWindow.Text += "Неверно введен рейтинг выход за диапазон\n";
                return;
            }
            TextBoxRating.Text = ratio.ToString("#.##");
        }

        //Тест коректно ли введен рейтинг
        private bool TestRating()
        {
            float ratio;
            if (float.TryParse(TextBoxRating.Text, out ratio) == false)
            {
                TextBoxLogWindow.Text += "Неверно введен рейтинг\n";
                return false;
            }
            if (ratio < 0 || ratio > 10)
            {
                TextBoxLogWindow.Text += "Неверно введен рейтинг выход за диапазон\n";
                return false;
            }

            TextBoxRating.Text = ratio.ToString("#.##");
            return true;
        }

        //Выполняем все проверки проверяем правельность заполнения полей
        private bool CreateGameTest()
        {
            if (TextBoxTitleRu.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено поле название по русски\n";
                return false;
            }

            if (TextBoxTitleEn.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено поле название по английски\n";
                return false;
            }

            if (Directory.Exists(DstGameDirPath + "\\" + TextBoxCatalogName.Text) == false)
            {
                TextBoxLogWindow.Text += "Не существует директория\n";
                return false;
            }

            if (TestYearPublishing() == false) return false;

            if (TestRating() == false) return false;

            if (TextBoxNumOfPlayers.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено поле количество игроков\n";
                return false;
            }

            if (TextBoxNumOfSuggested.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено поле Рек. игроков\n";
                return false;
            }

            if (TextBoxSuggestedAges.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено поле возраст\n";
                return false;
            }


            if (TextBoxAcquaintance.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено время освоения\n";
                return false;
            }

            if (TextBoxPlayingTime.Text.Trim(' ').Length == 0)
            {
                TextBoxLogWindow.Text += "Не заполнено время партии\n";
                return false;
            }

            if (TextBoxDescript.Text.Contains("<p>") == false)
            {
                TextBoxLogWindow.Text += "Не заполнено описание игры\n";
                return false;
            }

            if (TextBoxComponents.Text.Contains("<li>") == false)
            {
                TextBoxLogWindow.Text += "Не заполнен состав коробки игры\n";
                return false;
            }

            return true;
        }


        //Создание записи с игрой переход к картинкам
        private void ButtonStep_Click(object sender, RoutedEventArgs e)
        {
            if (CreateGameTest() == false) return;
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();
            Game NewGame = new Game();

            GameCatalogName = TextBoxCatalogName.Text;
            NewGame.TitleRu = TextBoxTitleRu.Text;
            NewGame.TitleEn = TextBoxTitleEn.Text;
            NewGame.CatalogName = TextBoxCatalogName.Text;
            NewGame.YearPublishing = int.Parse(TextBoxYearPublishing.Text);
            NewGame.Rating = float.Parse(TextBoxRating.Text.ToString());
            NewGame.Lang = ((ComboBoxItem)(СomboBoxLang.Items[СomboBoxLang.SelectedIndex])).Content.ToString();
            NewGame.Descript = TextBoxDescript.Text;
            NewGame.NumOfPlayers = TextBoxNumOfPlayers.Text;
            NewGame.NumOfSuggested = TextBoxNumOfSuggested.Text;
            NewGame.SuggestedAges = TextBoxSuggestedAges.Text;
            NewGame.Acquaintance = TextBoxAcquaintance.Text;
            NewGame.PlayingTime = TextBoxPlayingTime.Text;
            NewGame.Components = TextBoxComponents.Text;
            NewGame.CreateTime = DateTime.Now;

            GameEntities.Game.Add(NewGame);

            try
            {
                GameEntities.SaveChanges();

            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }

            GameID = GameEntities.Game.SqlQuery($"SELECT * FROM Game WHERE TitleRu='{TextBoxTitleRu.Text}'").First().GameID;
            TextBoxLogWindow.Text += $"Добавлена запись с ID {GameID} \n";

            ChangeToGameImage();

        }

        //Переключение на второй экран (работа с картинками)
        private void ChangeToGameImage()
        {
            GridCreateGame.Visibility = Visibility.Hidden;
            GridGameImage.Visibility = Visibility.Visible;
            ButtonStep.Content = "Далее >>";
        }

        #endregion


        #region второй экран (установка картинок)

        //Сканирование директории поиск всех файлов .jpg показ их на экране
        private void ButtonScanImage_Click(object sender, RoutedEventArgs e)
        {

            //----------------------------копирование файлов

            ImageListJpg = Directory.GetFiles(GameList[GameListIndex], "*.jpg").ToList();
            foreach (string image in ImageListJpg)
            {
                File.Move(image, DstGameDirPath + "\\" + GameCatalogName + "\\" + GameCatalogName + " " + Path.GetFileName(image));
                TextBoxLogWindow.Text += $"Перемещен файл {image} \n";
            }
            TextBoxLogWindow.Text += $"/n Всего перемещено файлов {ImageListJpg.Count} \n";



            //-----------------------------отображаем скопированное на экране

            TextBoxImage0.Text = "Коробка";
            //TextBoxImage1.Text =TextBoxImage2.Text =TextBoxImage3.Text =TextBoxImage4.Text =TextBoxImage5.Text =TextBoxImage6.Text =TextBoxImage7.Text = TextBoxImage8.Text = TextBoxImage9.Text = TextBoxTitleEn.Text;
            TextBoxImage1.Text = TextBoxImage2.Text = TextBoxImage3.Text = TextBoxImage4.Text = TextBoxImage5.Text = TextBoxImage6.Text = TextBoxImage7.Text = TextBoxImage8.Text = TextBoxImage9.Text = "Fairy Tale";

            WinImage.Add(Image0); WinImage.Add(Image1); WinImage.Add(Image2); WinImage.Add(Image3);
            WinImage.Add(Image4); WinImage.Add(Image5); WinImage.Add(Image6); WinImage.Add(Image7);
            WinImage.Add(Image8); WinImage.Add(Image9);


            ImageListJpg = Directory.GetFiles(DstGameDirPath + "\\" + GameCatalogName, "*.jpg").ToList();
            ImageListJpg.Sort((s, s1) => s.CompareTo(s1));

            for (int i = 0; i < 10; i++)
            {
                if (i >= ImageListJpg.Count) break;
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(ImageListJpg[i]);
                b.EndInit();
                WinImage[i].Source = b;
            }

            TextBoxLogWindow.Text += $"Загруженно {ImageListJpg.Count}  изображений" + "\n";


        }

        
        // конвертация изображений
        private void ButtonCreateSmallImage_Click(object sender, RoutedEventArgs e)
        {
            Process cmd = new System.Diagnostics.Process();
            string MagikConfig = "-resize 300x300  -gravity center -extent 300x300 -transparent white  -define png:compression-filter=5 -define png:compression-level=9 -define png:compression-strategy=1 -define png:exclude-chunk=all -interlace none -colorspace sRGB";

            cmd.StartInfo.WorkingDirectory = @"D:\tool\ImageMagik\";
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            foreach (string img in ImageListJpg)
            {
                cmd.StartInfo.Arguments = "/c " +
                                          $@"convert.exe ""{img}"" {MagikConfig} ""{Path.ChangeExtension(img, ".png")}"" ";
                cmd.Start();
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();

            }
            TextBoxLogWindow.Text += cmd.StandardOutput.ReadToEnd() + "\n";

            //показ сконвертированных изображений

            ImageListPng = Directory.GetFiles(DstGameDirPath + "\\" + GameCatalogName, "*.png").ToList();
            ImageListPng.Sort((s, s1) => s.CompareTo(s1));

            for (int i = 0; i < 10; i++)
            {
                if (i >= ImageListJpg.Count) break;
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(ImageListJpg[i]);
                b.EndInit();
                WinImage[i].Source = b;
            }

            TextBoxLogWindow.Text += $"Сконвертированно {ImageListPng.Count}/{ImageListJpg.Count}" + "\n";

        }

        private void ButtonToSql_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> TextBoxImgDesc = new List<TextBox>();
            TextBoxImgDesc.Add(TextBoxImage0); TextBoxImgDesc.Add(TextBoxImage1); TextBoxImgDesc.Add(TextBoxImage2);
            TextBoxImgDesc.Add(TextBoxImage3); TextBoxImgDesc.Add(TextBoxImage4); TextBoxImgDesc.Add(TextBoxImage5);
            TextBoxImgDesc.Add(TextBoxImage6); TextBoxImgDesc.Add(TextBoxImage7); TextBoxImgDesc.Add(TextBoxImage8); TextBoxImgDesc.Add(TextBoxImage9);


            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            for (int i = 0; i < ImageListJpg.Count; i++)
            {
                GameImage GameImage = new GameImage();
                GameImage.FulllImagePath = ImageListJpg[i].Substring(DstGameDirPath.Length);
                GameImage.SmallImagePath = ImageListPng[i].Substring(DstGameDirPath.Length); ;
                GameImage.DescriptImage = TextBoxImgDesc[i].Text;
                GameImage.GameID = GameID;

                GameEntities.GameImage.Add(GameImage);

                try
                {
                    GameEntities.SaveChanges();
                    TextBoxLogWindow.Text += $"В базу добавленно {Path.GetFileNameWithoutExtension(ImageListJpg[i])} " + "\n";
                }
                catch (Exception sql)
                {
                    MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                    return;
                }
            }

            TextBoxLogWindow.Text += $"В добавленно {ImageListJpg.Count} картинок" + "\n";

        }

        #endregion


        private void ButtonTag_Click(object sender, RoutedEventArgs e)
        {
            GridGameImage.Visibility = Visibility.Hidden;
            GridTag.Visibility = Visibility.Visible;
            TagRefresh();
        }

        #region Тэг

        //добавить тэг к списку тегов
        private void ButtonAddTag_Click(object sender, RoutedEventArgs e)
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            GameEntities.Tag.Add(new Tag() {TagName = TextBoxAddTag.Text});
            try
            {
                GameEntities.SaveChanges();
                TextBoxLogWindow.Text +=$"Тэг {TextBoxAddTag.Text} добавлен \n";
            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
            TagRefresh();
        }

        //Добавит тэг к игре
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if(ListBoxTag.SelectedIndex <0)return;
            Tag t=(Tag)((ListBoxItem) (ListBoxTag.Items[ListBoxTag.SelectedIndex])).Tag;
            ListBoxGameTag.Items.Add(new ListBoxItem() { Tag = t, Content = t.TagName });
        }

        //удалить тег у игры
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if(ListBoxGameTag.SelectedIndex<0)return;
            ListBoxGameTag.Items.RemoveAt(ListBoxGameTag.SelectedIndex);
        }

        
        // очистить фильтр тегов
        private void ButtonFindTagClear_Click(object sender, RoutedEventArgs e)
        {

        }

        // обновить список тегов
        private void TagRefresh()
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();
            TagList=GameEntities.Tag.ToList();

            ListBoxTag.Items.Clear();
            foreach (Tag tag in TagList)
            {
                ListBoxTag.Items.Add(new ListBoxItem() {Tag = tag, Content = tag.TagName});
            }

        }

        private void ButtonSaveTagToGame_Click(object sender, RoutedEventArgs e)
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            foreach (ListBoxItem item in ListBoxGameTag.Items)
            {
                GameEntities.GameTag.Add(new GameTag() {GameID = this.GameID, TagID = ((Tag) (item.Tag)).TagID});
            }
            try
            {
                GameEntities.SaveChanges();
                TextBoxLogWindow.Text += $"Тэги добавлены"; 
            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
            

        }

        #endregion


        private void ButtonNextGame_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
