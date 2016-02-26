using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kostil1
{
    class Program
    {
        static void Main(string[] args)
        {
            //PrintGameDataEntities enties =new PrintGameDataEntities();
            //foreach (GameImage gameImage in enties.GameImage)
            //{
            //    gameImage.FulllImagePath=gameImage.FulllImagePath.Replace('\\', '/');
            //    gameImage.SmallImagePath= gameImage.SmallImagePath.Replace('\\', '/');
            //}
            //enties.SaveChanges();


            //ImageMagik.JPGConvert(@"d:\temp\1.jpg", @"d:\temp\11.jpg");
            //ImageMagik.AddWatermark(@"d:\temp\1.jpg", @"d:\temp\11.jpg");
            //ImageMagik.CreatePNG(@"d:\temp\1.jpg", @"d:\temp\11.png");

            //List<string> files=Directory.GetFiles(@"D:\project\PrintGame\PrintGame\Content\GameData\", "*.jpg", SearchOption.AllDirectories).ToList();

            //foreach (string file in files)
            //{
            //    Console.WriteLine(file);
            //    FileInfo  f=new FileInfo(file);
            //    if (f.Length > 600000)
            //    {
            //        ImageMagik.JPGConvert(file, file);
            //        Console.WriteLine("--CONVERT--"+file);
            //    }
            //    ImageMagik.AddWatermark(file, file);
            //    Console.WriteLine("--WATEMARK--" + file);
            //}



            Console.ReadLine();
        }
    }


    static class ImageMagik
    {
        private static string options =
            "-define png:compression-filter=5 -define png:compression-level=9 -define png:compression-strategy=1 -define png:exclude-chunk=all -interlace none -colorspace sRGB ";

        private static string CreatePNGCmd = "-resize 300x300  -gravity center -extent 300x300 -transparent white ";

        private static string ResizeJPGCmd =
            "-strip -gaussian-blur 0.05 -quality 80 -resize x1024  -gravity center -transparent white ";

        private static string AddWatermarkCmd = "-gravity SouthEast -watermark 40 ";

        private static string WatermarkImg = @"D:\tool\ImageMagik\logo-main.png ";

        private static string WorkingDirectory = @"D:\tool\ImageMagik\";

        
        public static void JPGConvert(string src, string dst)
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = WorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C convert.exe " +  ResizeJPGCmd + options + " \"" + src + "\" \"" + dst+"\"";
            cmd.Start();
            
            cmd.WaitForExit();
            
        }


        public static void CreatePNG(string src, string dst)
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = WorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C convert.exe " + CreatePNGCmd + options + " \"" + src + "\" \"" + dst + "\"";
            cmd.Start();

            cmd.WaitForExit();

        }


        public static void AddWatermark(string src, string dst)
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = WorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C composite.exe " + AddWatermarkCmd + WatermarkImg + options + " \"" + src + "\" \"" + dst + "\"";
            cmd.Start();

            cmd.WaitForExit();
        }

    }

}
