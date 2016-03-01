using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintGame.Areas.Admin.Controllers
{
    public class TestController : Controller
    {
        //Проверка целостности картинок
        [Authorize(Roles = "Admin")]
        public ActionResult Image()
        {
            string DstGameDirPath = @"D:\Projects\PrintGame\PrintGame\Content\GameData\";
                //Directory.GetCurrentDirectory();


            PrintGameDataEntities enties = new PrintGameDataEntities();
            int CountDataBaseImage = enties.GameImage.Count() * 2;

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



            List<string> FileInDisk = Directory.GetFiles(DstGameDirPath, "*.*", SearchOption.AllDirectories).ToList();
            int CountDiskImage = FileInDisk.Count;

            for (int i = 0; i < FileInDisk.Count; i++)
            {
                FileInDisk[i] = FileInDisk[i].Remove(0, DstGameDirPath.Length - 1).Replace("\\", "/");
            }


            List<ImageModel> FileInDataBaseResault = new List<ImageModel>();

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

            ViewBag.FileInDataBaseResault = FileInDataBaseResault;
            ViewBag.FileInDisk = FileInDisk;

            ViewBag.CountDiskImage = CountDiskImage;
            ViewBag.CountDataBaseImage = CountDataBaseImage;

            return View();
        }
    }

    public class ImageModel
    {
        public int GameID;
        public string ImagePath;
    }


}