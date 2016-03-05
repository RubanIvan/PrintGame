using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CMS20.Model;

namespace CMS20.FileShareManage
{
    class DepositFileShare
    {
       //авторизация на сервере
        public async Task<CookieCollection> FirstLogin()
        {
            //---1 запрос на главную втраницу за куками

            //обращаемся на главную страницу
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dfiles.ru/");
            CookieCollection cookiesStartPage = new CookieCollection();
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookiesStartPage);
            //получаем первые куки от сервера
            HttpWebResponse response =  (HttpWebResponse) await request.GetResponseAsync();
            cookiesStartPage = response.Cookies;

            //---2 запрос непосредственно авторизация
            string AuthUrl = "http://dfiles.ru/api/user/login";
            string PostData = "login=print-game&password=Hd5Xky3D4O&recaptcha_challenge_field=&recaptcha_response_field=&g-recaptcha-response=";

            request = (HttpWebRequest)HttpWebRequest.Create(AuthUrl);
            request.Method = WebRequestMethods.Http.Post;

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookiesStartPage); //подставляем куки полученные ранее

            byte[] byteArray = Encoding.ASCII.GetBytes(PostData);
            request.ContentLength = byteArray.Length;

            Stream newStream = request.GetRequestStream();      //Открываем соединения
            newStream.Write(byteArray, 0, byteArray.Length);    //Посылаем данные

            response = (HttpWebResponse)await request.GetResponseAsync();  //получаем ответ

            //Проверка получена ли кука авторизации
            string LoginTest = "";
            foreach (Cookie cookie in response.Cookies)
                LoginTest += cookie.Name;

            
            if (LoginTest.Contains("autologin") == false)
            {
                throw new Exception("Login error");
            }

            //возвращаем куки от сервера
            return response.Cookies;
        }

        //получение списка загруженных файлов
        public  DepositFileArray GetFile(CookieCollection AuthCookie)
        {
            //запрашиваем данные

            HttpWebRequest requestJSON = (HttpWebRequest)HttpWebRequest.Create("http://dfiles.ru/gold/files_list.php?format=json&s[id_file_folder]=_root&page=1&res_type=array");
            requestJSON.Method = WebRequestMethods.Http.Get;
            requestJSON.CookieContainer = new CookieContainer();
            requestJSON.CookieContainer.Add(AuthCookie);
            //requestJSON.CookieContainer.Add(new Cookie("cookie", "1","/", AuthCookie[0].Domain));
            //requestJSON.CookieContainer.Add(new Cookie("lang_current", "ru", "/", AuthCookie[0].Domain));
            //requestJSON.CookieContainer.Add(new Cookie("dfsharedfiles_ActiveFolder", "sharedfiles", "/", AuthCookie[0].Domain));
            //requestJSON.CookieContainer.Add(new Cookie("order_param", "asc", "/", AuthCookie[0].Domain));
            //requestJSON.CookieContainer.Add(new Cookie("sort_param", "name", "/", AuthCookie[0].Domain));
            //requestJSON.CookieContainer.Add(new Cookie("Referer", @"http://dfiles.ru/gold/files_list.php", "/", AuthCookie[0].Domain));

            requestJSON.Accept = "application/json, text/javascript, */*; q=0.01";
            requestJSON.UserAgent =@"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36";
            requestJSON.Headers.Add("X-Requested-With", "XMLHttpRequest");
            requestJSON.Referer = "http://dfiles.ru/gold/files_list.php";
            requestJSON.Timeout = 1000 * 30;
            requestJSON.Proxy = null;
            //получаем ответ


            HttpWebResponse response = (HttpWebResponse)requestJSON.GetResponse();
           
            
            StringBuilder Json = new StringBuilder();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                // собираем ответ до кучи
                string line;
                while ((line = stream.ReadLine()) != null)
                    Json.Append(line);

            }

            return JsonConvert.DeserializeObject<DepositFileArray>(Json.ToString());

        }

       

    }

    

}
