using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            W.TextBoxRating.LostFocus += TextBoxRating_LostFocus;
        }


        private void TextBoxRating_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        /// <summary>Проверка ввели ли правельно год</summary>
        private void TextBoxYearPublishing_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (W.TextBoxYearPublishing.Text == "") return;
            int year = 0;
            //Пробуем перевести в число
            if (int.TryParse(W.TextBoxYearPublishing.Text, out year) == false)
            {
                //перевод не удался стираем неверное значение
                W.TextBoxYearPublishing.Text = "";
                return;
            }
            //если указан короткий год то дополняем его
            if (year >= 70 && year <= 99) W.TextBoxYearPublishing.Text = (1900 + year).ToString();
            if (year >= 0 && year <= 20) W.TextBoxYearPublishing.Text = (2000 + year).ToString();
        }
    }
}
