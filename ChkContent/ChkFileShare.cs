using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

            CookieCollection CookiesAuth = new CookieCollection();
            CookiesAuth = response.Cookies;

            //запрашиваем данные

            HttpWebRequest requestJSON = (HttpWebRequest)HttpWebRequest.Create("http://dfiles.ru/gold/files_list.php?format=json&s[id_file_folder]=_root&page=1&res_type=array");
            requestJSON.Method = WebRequestMethods.Http.Get;
            requestJSON.CookieContainer = new CookieContainer();
            requestJSON.CookieContainer.Add(CookiesAuth);
            //requestJSON.CookieContainer.Add(new Cookie())
            requestJSON.Accept = "application/json, text/javascript, */*; q=0.01";
            requestJSON.Referer = "http://dfiles.ru/gold/files_list.php";

            response = (HttpWebResponse)requestJSON.GetResponse();
            
            StringBuilder Json=new StringBuilder();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                // Выводим исходный код страницы
                string line;
                while ((line = stream.ReadLine()) != null)
                    Json.Append(line);
                //Console.WriteLine(line);
            }

            var  ff = JsonConvert.DeserializeObject<JsonFiles>(Json.ToString());

            Console.WriteLine(Json);

        }
    }

    public class JsonFiles
    {
        [JsonProperty("Files")]
        public JsonFile[] Files;
    }

    public class JsonFile
    {
        [JsonProperty("download_cnt")   ]
        public long download_cnt;       //download_cnt=0

        [JsonProperty("download_url")]
        public string download_url;     //download_url=http://dfiles.ru/files/usp50xnu6

        [JsonProperty("dt_added")]
        public DateTime dt_added;       //dt_added=2016-02-28 19:57:53

        [JsonProperty("dt_expires")]
        public DateTime dt_expires;     //dt_expires=2016-05-28 19:57:53

        [JsonProperty("file_password")]
        public string file_password;    //  

        [JsonProperty("filename_source")]
        public string filename_source;  //filename_source=Dungeon Lords1.zip

        [JsonProperty("id")]
        public long id;                 //id=166145650

        [JsonProperty("id_str")]
        public string id_str;           //id_str=usp50xnu6

        [JsonProperty("size")]
        public long size;               //size=261597700
    }
}
