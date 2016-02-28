using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace CMS20.Controllers
{
    class ParseUrl
    {
        private MainWindow W;

        public ParseUrl(MainWindow w)
        {
            W = w;
            W.ButtonParseUrl.Click += ButtonParseUrl_Click;
            W.TextBoxUrl.KeyDown += TextBoxUrl_KeyDown;

        }

        //обработка нажатия Enter
        private void TextBoxUrl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && W.TextBoxUrl.Text.Length>0)
            {
                ButtonParseUrl_Click(null,null);
            }
        }

        //Пытаемся получить данные с сервера
        private void ButtonParseUrl_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            W.ButtonParseUrl.IsEnabled = false;

            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(W.TextBoxUrl.Text);

            }
            catch (Exception url)
            {
                W.Log.Add(url.Message);
                W.ButtonParseUrl.IsEnabled = true;
            }

            request.Timeout = 10000;

            IAsyncResult result = request.BeginGetResponse(ar =>
            {

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);

                    Stream streamResponse = response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    string responseString = streamRead.ReadToEnd();
                    //Debug.WriteLine(responseString.Substring(0, 200));

                    streamResponse.Close();
                    streamRead.Close();
                    response.Close();
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { W.Log.Add("Данные получены"); }));
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { W.ButtonParseUrl.IsEnabled = true; }));
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { this.Parser(responseString); }));

                }
                catch (Exception UrlError)
                {
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { W.Log.Add("Ошибка получения данных"); }));
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { W.Log.Add(UrlError.Message); }));
                    W.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { W.ButtonParseUrl.IsEnabled = true; }));
                }

            }, request);
        }


        //парсим даные
        public void Parser(string data)
        {
            W.Log.Add("Парсим страницу");

            
            HtmlParser parser = new HtmlParser();
            IHtmlDocument doc = parser.Parse(data);

            try
            {
                W.TextBoxTitleRu.Text = doc.QuerySelector("#game_title span").TextContent;
            }
            catch (Exception)
            {
                W.Log.Add("Неудалось распарсить название");
            }


            try
            {
                W.TextBoxYearPublishing.Text = doc.QuerySelector("#games_games > div.gameholder > div.leftcol > h3").TextContent.Split(',').Last().Trim().Substring(0, 4);
            }
            catch (Exception)
            {

                W.Log.Add("Неудалось распарсить год");
            }

            try
            {
                W.TextBoxRating.Text = doc.QuerySelector(".colright div > h3 > span > span.bigrating").TextContent;
            }
            catch (Exception)
            {

                W.Log.Add("Неудалось распарсить рейтинг");
            }


            try
            {
                W.TextBoxNumOfPlayers.Text = doc.QuerySelector(" div.leftcol > ul > li:nth-child(1)").TextContent;
                W.TextBoxNumOfSuggested.Text = doc.QuerySelector(" div.leftcol > ul > li:nth-child(2)").TextContent;
                W.TextBoxSuggestedAges.Text = doc.QuerySelector(" div.leftcol > ul > li:nth-child(3)").TextContent.Replace("от", "").Replace("лет", "").Trim();
                W.TextBoxAcquaintance.Text = doc.QuerySelector(" div.leftcol > ul > li:nth-child(4)").TextContent.Replace("мин", "").Trim();
                W.TextBoxPlayingTime.Text = doc.QuerySelector(" div.leftcol > ul > li:nth-child(5)").TextContent.Replace("мин", "").Trim();
            }
            catch (Exception)
            {

                W.Log.Add("Неудалось распарсить параметры");
            }

            try
            {
                W.TextBoxComponents.Text = doc.QuerySelector("#games_games > div.gameholder > div.leftcol > div.cols.op > div.colmid > table > tbody > tr:nth-child(3) > td > p").InnerHtml.Replace("<br>", "\n");
            }
            catch (Exception e)
            {
                W.Log.Add("Комплектация не найдена");
            }

            try
            {
                W.TextBoxDescript.Text = doc.QuerySelector("#plsh1 > div").TextContent; //InnerHtml.Replace("<p>", "").Replace("</p>", "\n");
            }
            catch (Exception)
            {

                W.Log.Add("Описание не найдено");
            }



        }
    }
}
