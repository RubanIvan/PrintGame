using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGameDataEntities enties =new PrintGameDataEntities();

            DateTime SubDate=DateTime.Now.AddDays(-14);
            StringBuilder EmailText=new StringBuilder();

            foreach (Subscribe subscribe in enties.Subscribe)
            {
                Console.WriteLine($"Обрабатываем подписку для ящика {subscribe.Email}");
                string[] SubscribeTag = JsonConvert.DeserializeObject<string[]>(subscribe.TagList);
                EmailText.Clear();

                foreach (string SubTag in SubscribeTag)
                {
                    var game = from e in enties.Game
                        join gt in enties.GameTag on e.GameID equals gt.GameID
                        join tt in enties.Tag on gt.TagID equals tt.TagID
                        where tt.TagName == SubTag && e.CreateTime>SubDate
                        select e;
                    if (game.Any())
                    {
                        EmailText.Append($"\n\n пополнение игр с тегом <{SubTag}> \n");
                        foreach (Game game1 in game)
                        {
                            EmailText.Append($"{game1.CreateTime.ToString("g")}  {game1.TitleRu} ({game1.TitleEn}), {game1.YearPublishing} \n");
                        }
                    }
                    
                }
                Console.WriteLine(EmailText.ToString());
                SubscibeMessage msg=new SubscibeMessage();
                msg.Destination = subscribe.Email;
                msg.Body = EmailText.ToString();
                msg.Subject = "Поступление новых игр";
                SendAsync(msg);
            }
            Console.ReadLine();
        }


        public async static void SendAsync(SubscibeMessage message)
        {
            // настройка логина, пароля отправителя
            var from = "subscribe@print-game.ru";
            var pass = "kmD487Zsaof9";

            

            // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
            SmtpClient client = new SmtpClient("smtp.yandex.ru", 25);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            // создаем письмо: message.Destination - адрес получателя
            var mail = new MailMessage(from, message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            //client.Send(mail);
            await client.SendMailAsync(mail);
        }

    }


    public class SubscibeMessage
    {
        public string Destination;

        public string Subject;

        public string Body;
    }
}
