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

            #region file convert 2

            foreach (string directory in Directory.GetDirectories("D:\\npr\\npr\\"))
            {
                foreach (string NumDir in Directory.GetDirectories(directory))
                {
                    string src = NumDir;
                    
                    
                    if (Directory.GetDirectories(NumDir).Length == 1 && Directory.GetFiles(NumDir).Length==0)
                    {
                        Console.WriteLine(NumDir);
                        string deep = Directory.GetDirectories(NumDir)[0];
                        MoveDirectory(deep, NumDir);
                        //MoveDirectory(Directory.GetDirectories(NumDir)[0],
                        //  Directory.GetParent(Directory.GetDirectories(NumDir)[0].ToString());

                    }
                }
            }

            #endregion

            #region file convert

            //string SrcDir= @"D:\npr\npr\";
            //string DstDir= @"D:\Game_Base_all\";

            //List<string> DirList = new List<string>();
            //DirList = Directory.GetDirectories(SrcDir).ToList();
            //DirList.Sort();

            //var game = (from d in DirList
            //        select  d.Split('(').First().Trim()).Distinct();


            //foreach (string g in game)
            //{
            //    var ga = from dl in DirList
            //        where dl.Contains(g)
            //        select dl;

            //    Console.WriteLine(g+" : "+ga.Count());
            //    string NewDirName = Path.Combine(DstDir, Path.GetFileName(g));
            //    Directory.CreateDirectory(NewDirName);
            //    int i = 0;
            //    foreach (string oldgamedir in ga)
            //    {
            //        //Directory.CreateDirectory(Path.Combine(NewDirName, i.ToString()));
            //        Directory.Move(oldgamedir+"\\", Path.Combine(NewDirName, i.ToString()));
            //        i++;
            //    }

            //}


            #endregion

            #region 
            //FileShare fs = new FileShare();
            //fs.GameID = 35;
            //fs.FileShareDesc = "Dominant Species";
            //fs.FileShareName = "Dominant Species0.zip";
            //fs.FileShareSize = 13466336;

            //FileShare fs1 = new FileShare();
            //fs1.GameID = 35;
            //fs1.FileShareDesc = "Dominant Species";
            //fs1.FileShareName = "Dominant Species1.zip";
            //fs1.FileShareSize = 56573319;

            //FileShare fs2 = new FileShare();
            //fs2.GameID = 35;
            //fs2.FileShareDesc = "Dominant Species";
            //fs2.FileShareName = "Dominant Species2.zip";
            //fs2.FileShareSize = 54716272;

            //PrintGameDataEntities enties = new PrintGameDataEntities();
            //enties.FileShare.Add(fs);
            //enties.FileShare.Add(fs1);
            //enties.FileShare.Add(fs2);

            //enties.SaveChanges();
            #endregion
            #region 
            //PrintGameDataEntities enties = new PrintGameDataEntities();
            //foreach (GameImage gameImage in enties.GameImage)
            //{
            //    gameImage.FulllImagePath = gameImage.FulllImagePath.Replace('\\', '/');
            //    gameImage.SmallImagePath = gameImage.SmallImagePath.Replace('\\', '/');
            //}
            //enties.SaveChanges();

            #endregion
            #region 
            //PrintGameDataEntities enties = new PrintGameDataEntities();
            //List<string> files= Directory.GetFiles(@"D:\Content\FileShare\", "*.zip", SearchOption.AllDirectories).ToList();


            //foreach (string file in files)
            //{
            //    Console.WriteLine(file);
            //    string fname = Path.GetFileName(file);
            //    FileInfo f = new FileInfo(file);
            //    enties.FileShare.First(fs => fs.FileShareName == fname).FileShareSize =f.Length;
            //}
            //enties.SaveChanges();
            #endregion
            #region 
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
            #endregion

            Console.WriteLine("---------------------------!! End !!-----------------------------");
            Console.ReadLine();
        }


        public static void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            Directory.Delete(source, true);
        }

        public class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
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
