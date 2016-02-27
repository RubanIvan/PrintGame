using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS20.Controllers
{
    public class Test
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        public Test(MainWindow w)
        {
            W = w;
        }


        public bool TestGame()
        {
            if (W.TextBoxTitleRu.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено поле название по русски");
                return false;
            }

            if (W.TextBoxTitleEn.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено поле название по английски");
                return false;
            }

            if (Directory.Exists(W.DstGameDirPath + W.TextBoxCatalogName.Text) == false)
            {
                W.Log.Add("Не существует директория");
                return false;
            }

            if(TestYearPublishing()==false)return false;


            if(TestRating()==false)return false;

            if (W.TextBoxNumOfPlayers.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено поле количество игроков");
                return false;
            }

            if (W.TextBoxNumOfSuggested.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено поле Рек. игроков");
                return false;
            }

            if (W.TextBoxSuggestedAges.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено поле возраст");
                return false;
            }


            if (W.TextBoxAcquaintance.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено время освоения");
                return false;
            }

            if (W.TextBoxPlayingTime.Text.Trim(' ').Length == 0)
            {
                W.Log.Add("Не заполнено время партии");
                return false;
            }

            if (W.TextBoxDescript.Text.Contains("<p>") == false)
            {
                W.Log.Add("Не заполнено описание игры. Нет тега <p>");
                return false;
            }

            if (W.TextBoxComponents.Text.Contains("<li>") == false)
            {
                W.Log.Add("Не заполнен состав коробки игры. Нет тега <li>");
                return false;
            }
            return true;
        }

       

        //Тест коректно ли введен год
        public bool TestYearPublishing()
        {
            int year = 0;
            if (int.TryParse(W.TextBoxYearPublishing.Text, out year) == false)
            {
                W.Log.Add("Неверно введен год");
                return false;
            }
            //проверка попадания в диапозон
            if (year < 1970 || year > 2020)
            {
                W.Log.Add("Неверно введен год ошибка диапозона");
                return false;
            }
            return true;
        }


        //Тест рэйтинга
        public bool TestRating()
        {
            W.TextBoxRating.Text = W.TextBoxRating.Text.Replace('.', ',');
            float ratio;
            if (float.TryParse(W.TextBoxRating.Text, out ratio) == false)
            {
                W.Log.Add("Неверно введен рейтинг");
                return false;
            }
            if (ratio < 0 || ratio > 10)
            {
                W.Log.Add("Неверно введен рейтинг выход за диапазон");
                return false;
            }
            return true;
        }


    }
}
