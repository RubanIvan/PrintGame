using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CMS20.Properties;

namespace CMS20.FileShareManage
{
    class FTP
    {
        private string ftp_login = "print-game";
        private string trp_pass = "pug1cwp1";
        private string ftp_url = "ftp://ftpupload1.dfiles.ru/";
        public string FileShareFolder = Settings.Default.FileShareFolder;// @"D:\Content\FileShare\";

        public async void Upload(string filename, Action<int> Progress)
        {
            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftp_url+ filename);
            ftpWebRequest.Credentials = new System.Net.NetworkCredential(ftp_login, trp_pass);
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = true;
            ftpWebRequest.UsePassive = true;

            using (var inputStream = File.OpenRead( Path.Combine(FileShareFolder,filename)))
            using (var outputStream = ftpWebRequest.GetRequestStream())
            {
                var buffer = new byte[1024 * 1024];
                int totalReadBytesCount = 0;
                int readBytesCount;
                while ((readBytesCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await outputStream.WriteAsync (buffer, 0, readBytesCount);
                    //outputStream.Write(buffer, 0, readBytesCount);
                    totalReadBytesCount += readBytesCount;
                    var progress = totalReadBytesCount * 100.0 / inputStream.Length;
                    Progress((int)progress);
                    //backgroundWorker1.ReportProgress((int)progress);
                }
            }
        }

        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progressBar.Value = e.ProgressPercentage;
        //}


    }
}
