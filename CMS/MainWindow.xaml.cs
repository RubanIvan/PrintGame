using System;
using System.Collections.Generic;
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

        string CatalogPath = @"D:\Content\GameData\";
        string DirPath= @"D:\Game_Base";
        int GameID; //ID вставленной записи
        List<string> GameList=new List<string>();
        private int GameListIndex = 0; 

        public MainWindow()
        {
            InitializeComponent();
            SetDir();
        }

        #region Установки каталогов

        private void SetDir()
        {
            LabelSetDir.Content = DirPath;
            GameList=Directory.GetDirectories(DirPath).ToList();
            ShowGameListIndex();
        }

        private void ButtonSetDir_Click(object sender, RoutedEventArgs e)
        {}


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
            string dirname=System.IO.Path.GetFileName(GameList[GameListIndex]);
            TextBoxTitleEn.Text = dirname;
            TextBoxCatalogName.Text = dirname;
        }

        //Создание директории для картинок и копирование картинок которые там есть
        private void ButtonCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCatalogName.Text == "")
            {
                TextBoxLogWindow.Text += "Неверно указано имя дериктории\n";
                return;
            }

            try
            {
                Directory.CreateDirectory(CatalogPath + "\\" + TextBoxCatalogName.Text);
            }
            catch (Exception ee)
            {
                TextBoxLogWindow.Text += "Ошибка создания дериктории\n" + ee.Message + "\n";
                return;
            }

            TextBoxLogWindow.Text += "Директория создана\n" + TextBoxCatalogName.Text + "\n";

            //----------------------------копирование файлов

            List<string> ImageList=Directory.GetFiles(GameList[GameListIndex], "*.jpg").ToList();
            foreach (string image in ImageList)
            {
                File.Move(image, CatalogPath + TextBoxCatalogName.Text+"\\"+TextBoxCatalogName.Text+" "+ Path.GetFileName(image));
                TextBoxLogWindow.Text += $"Перемещен файл {image} \n";
            }
            TextBoxLogWindow.Text += $"/n Всего перемещено файлов {ImageList.Count} \n";


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

            if (Directory.Exists(CatalogPath + "\\" + TextBoxCatalogName.Text) == false)
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
                MessageBox.Show("Error", sql.Message, MessageBoxButton.OK);
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

        //Сканирование директории поиск всех файлов .jpg
        private void ButtonScanImage_Click(object sender, RoutedEventArgs e)
        {

        }






        #endregion


    }
}
