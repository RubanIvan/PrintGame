using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChkContent
{
    class ChkFileShare
    {
        public ChkFileShare()
        {

        }

        public void Chk()
        {
            string AuthUrl = "http://dfiles.ru/api/user/login";
            string PostData = "login=print-game&password=Hd5Xky3D4O&recaptcha_challenge_field=&recaptcha_response_field=&g-recaptcha-response=";

            //обращаемся на главную страницу
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dfiles.ru/");
            CookieCollection cookies = new CookieCollection();
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies);
            //получаем первые куки от сервера
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookies = response.Cookies;


            // Запрос к странице авторизации
            request = (HttpWebRequest)HttpWebRequest.Create(AuthUrl);
            request.Method = WebRequestMethods.Http.Post;
             
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer=new CookieContainer();
            request.CookieContainer.Add(cookies); //подставляем куки полученные ранее

            byte[] byteArray = Encoding.ASCII.GetBytes(PostData);
            request.ContentLength=byteArray.Length;

            Stream newStream = request.GetRequestStream(); //open connection
            newStream.Write(byteArray, 0, byteArray.Length); // Send the data.


            response = (HttpWebResponse)request.GetResponse();

            string LoginTest = "";

            foreach (Cookie cookie  in response.Cookies)
                LoginTest += cookie.Name;

            #region данные из ответа сервера
            //using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            //{
            //    // Выводим исходный код страницы
            //    string line;
            //    while ((line = stream.ReadLine()) != null)
            //        Console.WriteLine(line);
            //}
            #endregion

            //Проверка получена ли кука авторизации
            if (LoginTest.Contains("autologin") == false)
            {
                Console.WriteLine("Ощибка авторизации");
                return;
            }

            

            
            Console.WriteLine(response.Cookies.Count);

        }
    }
}
