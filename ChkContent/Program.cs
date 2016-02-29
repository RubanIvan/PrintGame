using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChkContent
{
    class Program
    {
        static void Main(string[] args)
        {
            ChkImage image=new ChkImage();
            image.Chk();

            Console.ReadLine();

        }
    }

    class ChkImage
    {

        /// <summary>каталог КУДА будут скопированы файлы</summary>
        public string DstGameDirPath = @"D:\Content\GameData\";


        public ChkImage()
        {
            
        }

        public void Chk()
        {
            PrintGameDataEntities enties =new PrintGameDataEntities();
            int CountDataBase = enties.GameImage.Count()*2;

            List<ImageModel> FileInDataBase = (from gi in enties.GameImage
                select new ImageModel()
                {
                    GameID = gi.GameID.Value,
                    ImagePath = gi.FulllImagePath
                }).ToList();

            FileInDataBase = FileInDataBase.Concat((from gii in enties.GameImage
                                select new ImageModel()
                                {
                                    GameID = gii.GameID.Value,
                                    ImagePath = gii.SmallImagePath
                                }).ToList()).ToList();


            
            List < string > FileInDisk = Directory.GetFiles(DstGameDirPath, "*.*", SearchOption.AllDirectories).ToList();
            int CountDisk = FileInDisk.Count;

            for (int i = 0; i < FileInDisk.Count; i++)
            {
                FileInDisk[i]=FileInDisk[i].Remove(0,DstGameDirPath.Length - 1).Replace("\\", "/");
            }


            List<ImageModel> FileInDataBaseResault=new List<ImageModel>();

            for (int i = 0; i < FileInDataBase.Count; i++)
            {
                int x = FileInDisk.FindIndex(f => f == FileInDataBase[i].ImagePath);
                if (x >= 0)
                {
                    FileInDisk.RemoveAt(x);
                    
                }
                else
                {
                    FileInDataBaseResault.Add(FileInDataBase[i]);
                }
            }

            Console.WriteLine("-------------------------Отсутствуют на диске----------------------------");

            foreach (ImageModel imageModel in FileInDataBaseResault)
            {
                Console.WriteLine($"GameID: {imageModel.GameID}   Path:{imageModel.ImagePath}");
            }

            Console.WriteLine();

            Console.WriteLine("-------------------------Мусор на диске ----------------------------");

            foreach (string file in FileInDisk )
            {
                Console.WriteLine(file);
            }

            Console.WriteLine($"CountDataBase: {CountDataBase}");
            Console.WriteLine($"CountDisk: {CountDisk}");
            

        }
    }

    public class ImageModel
    {
        public int GameID;
        public string ImagePath;
    }


}
