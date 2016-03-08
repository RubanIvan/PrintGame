using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGameDataEntities enties =new PrintGameDataEntities();

            DateTime SubDate=DateTime.Now.AddDays(-7);
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
            }
            Console.ReadLine();
        }
    }
}
