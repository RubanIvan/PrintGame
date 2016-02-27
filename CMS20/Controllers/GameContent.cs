using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;

namespace CMS20.Controllers
{
    class GameContent
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        public GameContent(MainWindow w)
        {
            W = w;
            W.TextBoxYearPublishing.LostFocus += TextBoxYearPublishing_LostFocus;
            W.TextBoxYearPublishing.KeyDown += TextBoxYearPublishing_KeyDown;
            W.TextBoxRating.LostFocus += TextBoxRating_LostFocus;
            W.TextBoxRating.KeyDown += TextBoxRating_KeyDown;
            W.ButtonTagP.Click += ButtonTagP_Click;
            W.ButtonTagLi.Click += ButtonTagLi_Click;
            W.ButtonCreateGame.Click += ButtonCreateGame_Click;
        }

        #region работа с годом

        /// <summary>Проверка ввели ли правельно год</summary>
        private void TextBoxYearPublishing_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (W.TextBoxYearPublishing.Text == "") return;
            if (TextBoxYearPublishing_TryCorrect() == true && W.Test.TestYearPublishing() == true)
            {
                return;
            }
            else
            {
                //e.Handled = true;
                //W.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => W.TextBoxYearPublishing.Focus()));
            }
        }

        /// <summary>Переход по enter </summary>
        private void TextBoxYearPublishing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TextBoxYearPublishing_TryCorrect() == true && W.Test.TestYearPublishing() == true)
                {
                    W.TextBoxRating.Focus();
                    return;
                }
                return;
            }
        }

        private bool TextBoxYearPublishing_TryCorrect()
        {
            int year = 0;
            //Пробуем перевести в число
            if (int.TryParse(W.TextBoxYearPublishing.Text, out year) == false)
            {
                //перевод не удался стираем неверное значение
                W.TextBoxYearPublishing.Text = "";
                return false;
            }
            //если указан короткий год то дополняем его
            if (year >= 70 && year <= 99) W.TextBoxYearPublishing.Text = (1900 + year).ToString();
            if (year >= 0 && year <= 20) W.TextBoxYearPublishing.Text = (2000 + year).ToString();
            return true;
        }
        #endregion

        #region работа с рейтингом
        private void TextBoxRating_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            W.Test.TestRating();
        }

        private void TextBoxRating_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (W.Test.TestRating() == true)
                {
                    W.TextBoxNumOfPlayers.Focus();
                }
            }
        }
        #endregion

        //Обрамление строк в параграф
        private void ButtonTagP_Click(object sender, RoutedEventArgs e)
        {
            //Если не нашли открывающего тега
            if (W.TextBoxDescript.Text.Contains("<p>") == false)
            {
                //то обрамляем строки в тег <p>..</p>
                string[] TextList = W.TextBoxDescript.Text.Split(new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                W.TextBoxDescript.Text = "";
                foreach (string t in TextList)
                {
                    W.TextBoxDescript.Text += "<p>" + t + "</p>\n";
                }

            }
        }

        //Обрамление строк в список
        private void ButtonTagLi_Click(object sender, RoutedEventArgs e)
        {
            //Если не нашли открывающего тега
            if (W.TextBoxComponents.Text.Contains("<li>") == false)
            {
                //то обрамляем строки в тег <li>..</li>
                string[] TextList = W.TextBoxComponents.Text.Split(new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                W.TextBoxComponents.Text = "";
                foreach (string t in TextList)
                {
                    W.TextBoxComponents.Text += "<li>" + t + "</li>\n";
                }

            }
        }

        //Пытаемся создать запись в таблице и перейти к следующему экрану
        private void ButtonCreateGame_Click(object sender, RoutedEventArgs e)
        {
            if (W.Test.TestGame() == false) return;
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();
            Game NewGame = new Game();

            W.GameCatalogName = W.TextBoxCatalogName.Text;
            NewGame.TitleRu = W.TextBoxTitleRu.Text;
            NewGame.TitleEn = W.TextBoxTitleEn.Text;
            NewGame.CatalogName =W.TextBoxCatalogName.Text;
            NewGame.YearPublishing = int.Parse(W.TextBoxYearPublishing.Text);
            NewGame.Rating = float.Parse(W.TextBoxRating.Text.ToString());
            NewGame.Lang = ((ComboBoxItem)(W.СomboBoxLang.Items[W.СomboBoxLang.SelectedIndex])).Content.ToString();
            NewGame.Descript = W.TextBoxDescript.Text;
            NewGame.NumOfPlayers = W.TextBoxNumOfPlayers.Text;
            NewGame.NumOfSuggested = W.TextBoxNumOfSuggested.Text;
            NewGame.SuggestedAges = W.TextBoxSuggestedAges.Text;
            NewGame.Acquaintance = W.TextBoxAcquaintance.Text;
            NewGame.PlayingTime = W.TextBoxPlayingTime.Text;
            NewGame.Components = W.TextBoxComponents.Text;
            NewGame.CreateTime = DateTime.Now;

            GameEntities.Game.Add(NewGame);

            try
            {
                GameEntities.SaveChanges();

            }
            catch (Exception Ent)
            {
                MessageBox.Show(Ent.Message, "Error", MessageBoxButton.OK);
                return;
            }

            W.GameID = GameEntities.Game.SqlQuery($"SELECT * FROM Game WHERE TitleRu='{W.TextBoxTitleRu.Text}'").First().GameID;
            W.Log.Add($"Добавлена запись с ID {W.GameID}");

            
        }

    }
}
