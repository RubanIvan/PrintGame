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
using System.Windows.Shapes;

namespace CMS
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {

        string CatalogPath= @"D:\Content\GameData\";

        public MainWindow()
        {
            InitializeComponent();
        }

        //Обрамление строк в параграф
        private void ButtonTagP_Click(object sender, RoutedEventArgs e)
        {
            //Если не нашли открывающего тега
            if (TextBoxDescript.Text.Contains("<p>") == false)
            {
                //то обрамляем строки в тег <p>..</p>
                string[] TextList = TextBoxDescript.Text.Split(new[] {"\r\n", "\r", "\n"},
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
                string[] TextList = TextBoxComponents.Text.Split(new[] {"\r\n", "\r", "\n"},
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

        //кол-во картинок геймплея
        private void TextBoxImageGameplay_LostFocus(object sender, RoutedEventArgs e)
        {
            TestImageGameplay();
        }
        //Тест картинок геймплея
        private bool TestImageGameplay()
        {
            if (TextBoxImageGameplay.Text == "") return false;
            int image = 0;
            //Пробуем перевести в число
            if (int.TryParse(TextBoxImageGameplay.Text, out image) == false)
            {
                //перевод не удался стираем неверное значение
                TextBoxImageGameplay.Text = "";
                TextBoxLogWindow.Text += "Неверно введено кол-во картинок геймплея\n";
                return false;
            }

            if (image < 0 || image > 6)
            {
                TextBoxImageGameplay.Text = "";
                TextBoxLogWindow.Text += "Неверно введено кол-во картинок геймплея выход за диапозон (0-5) \n";
                return false;
            }
            return true;
        }
        //кол-во картинок примеры сканов
        private void TextBoxImageScans_LostFocus(object sender, RoutedEventArgs e)
        {
            TestImageScans();
        }
        //Тест картинок примеры сканов
        private bool TestImageScans()
        {
            if (TextBoxImageScans.Text == "") return false;
            int image = 0;
            //Пробуем перевести в число
            if (int.TryParse(TextBoxImageScans.Text, out image) == false)
            {
                //перевод не удался стираем неверное значение
                TextBoxImageScans.Text = "";
                TextBoxLogWindow.Text += "Неверно введено кол-во картинок геймплея\n";
                return false;
            }

            if (image <= 0 || image > 10)
            {
                TextBoxImageScans.Text = "";
                TextBoxLogWindow.Text += "Неверно введено кол-во картинок геймплея выход за диапозон (1-10) \n";
                return false;
            }
            return true;
        }

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
                TextBoxLogWindow.Text += "Ошибка создания дериктории\n"+ee.Message+"\n";
                return;
            }

            TextBoxLogWindow.Text += "Директория создана\n" + TextBoxCatalogName.Text + "\n";
        }
    }
}
