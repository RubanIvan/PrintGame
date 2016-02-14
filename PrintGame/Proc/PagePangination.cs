using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Web;

namespace PrintGame.Proc
{
    public static class PagePangination
    {

        public static string GetPangination(int CurPage, int MaxPage)
        {

            StringBuilder PangLi = new StringBuilder();

            const int MaxCel = 10;                          //всего показано страниц
            const int JumpCel = 6;                          //кол-во страниц для перехода в крайнии положения
            const int IndentCel = (int)(MaxCel / 2.0) - 1;    //отступ с лева и справа

            string Query = @"/page/";

            string Page1Url = "/";

            //самое начало
            if (CurPage == 1)
            {
                PangLi.AppendLine(@"<li><span>&lt;&lt; Назад</span></li>");
                PangLi.AppendLine(@"<li class=""PangCurPage""><span>1</span></li>");

                for (int i = 2; i <= MaxCel; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");


                PangLi.AppendLine(@"<li><span>...</span></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{MaxPage}"">{MaxPage}</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage + 1}"">Вперед &gt;&gt; </a></li>");
            }

            //все еще в начале
            if (CurPage > 1 && CurPage <= JumpCel)
            {
                //создаем кнопка назад
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage - 1}"">&lt;&lt; Назад</a></li>");

                //создаем ссылки до текущай страницы
                for (int i = 1; i < CurPage; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //текущаая страница
                PangLi.AppendLine($@"<li class=""PangCurPage""><span>{CurPage}</span></li>");

                //добавляем остаток
                for (int i = CurPage + 1; i <= MaxCel; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //+ кнопка Вперед
                PangLi.AppendLine(@"<li><span>...</span></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{MaxPage}"">{MaxPage}</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage + 1}"">Вперед &gt;&gt; </a></li>");

            }

            //серидина
            if (CurPage > JumpCel && CurPage < (MaxPage - JumpCel))
            {
                //создаем кнопку назад
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage - 1}"">&lt;&lt; Назад</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{1}"">1</a></li>");
                PangLi.AppendLine(@"<li><span>...</span></li>");

                //отступаем в лево от текущей страницы
                for (int i = CurPage - IndentCel; i < CurPage; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //текущаая страница
                PangLi.AppendLine($@"<li class=""PangCurPage""><span>{CurPage}</span></li>");

                //отступаем в право от текущей страницы
                for (int i = CurPage + 1; i <= CurPage + IndentCel; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //+ кнопка Вперед
                PangLi.AppendLine(@"<li><span>...</span></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{MaxPage}"">{MaxPage}</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage + 1}"">Вперед &gt;&gt; </a></li>");
            }

            //почти в конце
            if (CurPage >= (MaxPage - JumpCel) && CurPage != MaxPage)
            {
                //создаем кнопку назад
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage - 1}"">&lt;&lt; Назад</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{1}"">1</a></li>");
                PangLi.AppendLine(@"<li><span>...</span></li>");


                //создаем ссылки до текущай страницы
                for (int i = MaxPage - MaxCel; i < CurPage; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //текущаая страница
                PangLi.AppendLine($@"<li class=""PangCurPage""><span>{CurPage}</span></li>");

                //добавляем остаток
                for (int i = CurPage + 1; i <= MaxPage; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //+ кнопка Вперед
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage + 1}"">Вперед &gt;&gt; </a></li>");

            }

            //в самом конце
            if (CurPage == MaxPage)
            {
                //создаем кнопку назад
                PangLi.AppendLine($@"<li><a href=""{Query}{CurPage - 1}"">&lt;&lt; Назад</a></li>");
                PangLi.AppendLine($@"<li><a href=""{Query}{1}"">1</a></li>");
                PangLi.AppendLine(@"<li><span>...</span></li>");


                //создаем ссылки до текущай страницы
                for (int i = MaxPage - MaxCel; i < CurPage; i++)
                    PangLi.AppendLine($@"<li><a href=""{Query}{i}"">{i}</a></li>");

                //текущаая страница
                PangLi.AppendLine($@"<li class=""PangCurPage""><span>{CurPage}</span></li>");

                //+ кнопка Вперед
                PangLi.AppendLine($@"<li><span>Вперед &gt;&gt; </span></li>");

            }
            return PangLi.ToString();
        }



    }
}