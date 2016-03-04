using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CMS20.Properties;

namespace CMS20.Controllers
{
    static class ImageMagik
    {
        private static string options =
            "-define png:compression-filter=5 -define png:compression-level=9 -define png:compression-strategy=1 -define png:exclude-chunk=all -interlace none -colorspace sRGB ";

        private static string CreatePNGCmd = "-resize 300x300  -gravity center -extent 300x300 -transparent white ";

        private static string ResizeJPGCmd =
            "-strip -gaussian-blur 0.05 -quality 80 -resize x1024  -gravity center -transparent white ";

        //private static string AddWatermarkCmd = "-gravity SouthEast -watermark 40 ";
        private static List<string> AddWatermarkCmd=new List<string>();

        private static string WatermarkImgPath = Settings.Default.WatermarkImgPath+" ";  //@"D:\tool\ImageMagik\logo-main.png ";

        private static string ImageMagikWorkingDirectory = Settings.Default.ImageMagikWorkingDirectory;//@"D:\tool\ImageMagik\";

       

        static ImageMagik()
        {
            AddWatermarkCmd.Add("-gravity Northwest -watermark 40 ");
            AddWatermarkCmd.Add("-gravity Northeast -watermark 40 ");
            AddWatermarkCmd.Add("-gravity Southwest -watermark 40 ");
            AddWatermarkCmd.Add("-gravity SouthEast -watermark 40 ");
            AddWatermarkCmd.Add("-gravity Center -watermark 40 ");
        }

        //сконвертировать jpg уменьшить размер и качество
        public static void JPGConvert(string src, string dst)   
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = ImageMagikWorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C convert.exe " + ResizeJPGCmd + options + " \"" + src + "\" \"" + dst + "\"";
            cmd.Start();

            cmd.WaitForExit();

        }

        //создать png нужного размера
        public static void CreatePNG(string src, string dst)
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = ImageMagikWorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C convert.exe " + CreatePNGCmd + options + " \"" + src + "\" \"" + dst + "\"";
            cmd.Start();

            cmd.WaitForExit();

        }

        //Наложить водянной знак на картинку
        public static void AddWatermark(string src, string dst,int position)
        {
            Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.WorkingDirectory = ImageMagikWorkingDirectory;
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.StartInfo.Arguments = " /C composite.exe " + AddWatermarkCmd[position] + WatermarkImgPath + options + " \"" + src + "\" \"" + dst + "\"";
            cmd.Start();

            cmd.WaitForExit();
        }

        

    }

}
