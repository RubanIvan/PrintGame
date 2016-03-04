using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CMS20.Controllers;
using CMS20.Model;

namespace CMS20.FileShareManage
{
    class FileManageGUI
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        /// <summary>Обьект для работы с удаленным сервером</summary>
        private DepositFileShare Deposit = new DepositFileShare();

        /// <summary>Куки авторизации</summary>
        private CookieCollection AuthCookie;

        /// <summary>Список  загруженных файлов на сервер</summary>
        private List<DepositFile> DepositFileList;

        public FileManageGUI(MainWindow w)
        {
            W = w;

            W.TextBoxDepLog.TextChanged += (sender, args) => { W.TextBoxDepLog.ScrollToEnd(); };
            W.ButtonSyncFile.Click += ButtonSyncFile_Click;
            W.ButtonFileUpload.Click += ButtonFileUpload_Click;
        }



        public async void ButtonSyncFile_Click(object sender, RoutedEventArgs e)
        {
            W.ButtonSyncFile.IsEnabled = false;
            W.WaitControl.Visibility = Visibility.Visible;

            W.TextBoxDepLog.Add(@"Подключение к серверу http://dfiles.ru/");
            try
            {
                AuthCookie = await Deposit.FirstLogin();
            }
            catch (Exception login_error)
            {
                W.TextBoxDepLog.Add("Ощибка авторизации на сервере");
                W.TextBoxDepLog.Add(login_error.Message);
                W.WaitControl.Visibility = Visibility.Hidden;
                W.ButtonSyncFile.IsEnabled = true;
                return;
            }

            W.TextBoxDepLog.Add("Авторизация прошла успешно");
            //-----------------------


            W.TextBoxDepLog.Add("Получаем список загруженных файлов");
            try
            {
                DepositFileList = Deposit.GetFile(AuthCookie).Files.ToList();
            }
            catch (Exception getfile_error)
            {
                W.TextBoxDepLog.Add("Ошибка получения данных от вервера");
                W.TextBoxDepLog.Add(getfile_error.Message);
                W.WaitControl.Visibility = Visibility.Hidden;
                W.ButtonSyncFile.IsEnabled = true;
                return;
            }

            W.WaitControl.Visibility = Visibility.Hidden;
            W.TextBoxDepLog.Add("Список файлов на сервере получен");

            //Общая статистика
            PrintGameDataEntities enties = new PrintGameDataEntities();
            W.TextBoxDepLog.Add($"Файлов в базе данных: {enties.FileShare.Count()} \n" +
                               $"Файлов на DepositFiles: {DepositFileList.Count} \n" +
                               $"Файлов без ссылок или просроченных{enties.FileShare.Where(f => f.FileShareUrl1 == null || f.FileShareExpire1 < DateTime.Now).Count()}");

            FileLinkUpdate();

            W.WaitControl.Visibility = Visibility.Hidden;
            W.ButtonSyncFile.IsEnabled = true;


        }


        //Обновление ссылок в базе данных
        private void FileLinkUpdate()
        {
            PrintGameDataEntities enties = new PrintGameDataEntities();

            foreach (var depFile in DepositFileList)
            {
                var q1 = (from f in enties.FileShare
                          where f.FileShareName == depFile.filename_source && f.FileShareSize == depFile.size && (f.FileShareUrl1 == null || f.FileShareExpire1 < depFile.dt_expires)
                          select f);
                if (q1.Count() == 1)
                {
                    if (q1.First().FileShareExpire1.Value.Date >= depFile.dt_expires.Date) continue;
                    q1.First().FileShareUrl1 = depFile.download_url;
                    q1.First().FileShareExpire1 = depFile.dt_expires;
                }

            }

            try
            {
                int upd = enties.SaveChanges();
                W.TextBoxDepLog.Add($"Обновлено записей: {upd}");
            }
            catch (Exception upd)
            {
                W.TextBoxDepLog.Add("Ошибка обновления базы данных");
                W.TextBoxDepLog.Add(upd.Message);
            }

        }


        //Загрузка файлов на сервер
        private void ButtonFileUpload_Click(object sender, RoutedEventArgs e)
        {
            //если нет списка файлов делаем синхронизацию
            if (DepositFileList == null) ButtonSyncFile_Click(null, null);

            W.ButtonFileUpload.IsEnabled = false;

            PrintGameDataEntities enties = new PrintGameDataEntities();
            DateTime dayTomorou = DateTime.Today.AddDays(+1);
            var query = from f in enties.FileShare
                        where f.FileShareUrl1 == null ||  f.FileShareExpire1 <= dayTomorou
                        select f;

            List<string> GameList = Directory.GetFiles(W.FileShareFolder).ToList();

            foreach (FileShare fileShare in query)
            {
                string GameFile = GameList.First(g => Path.GetFileName(g) == fileShare.FileShareName);
                W.TextBoxDepLog.Add(GameFile);
            }

        }





    }
}
