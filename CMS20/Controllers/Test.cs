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

            return true;
        }

        //Тест коректно ли введен год
        private bool TestYearPublishing()
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

    }
}
