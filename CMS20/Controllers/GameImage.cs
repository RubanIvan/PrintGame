using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CMS20.Controllers
{
    class ImageCreate
    {
        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        //Список картинок в директории
        private List<string> ImageListJpg = new List<string>();

        //ссылки на картинки на экране
        private List<Image> WindowImage = new List<Image>();
        //ссылки наподписи
        List<TextBox> TextBoxImgDesc = new List<TextBox>();

        

        private static RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        private static byte[] RandomBuff = new byte[256];

        //Получить случайное число от 0 до 4
        private static int WatermarkIndex()
        {
            Rng.GetBytes(RandomBuff);
            return (int)((((int)RandomBuff[127]) / 256.0f) * 5);
        }


        public ImageCreate(MainWindow w)
        {
            W = w;
            W.ButtonScanImage.Click += ButtonScanImage_Click;
            W.ButtonConvertImage.Click += ButtonConvertImage_Click;

            WindowImage.Add(W.Image0); WindowImage.Add(W.Image1); WindowImage.Add(W.Image2); WindowImage.Add(W.Image3);
            WindowImage.Add(W.Image4); WindowImage.Add(W.Image5); WindowImage.Add(W.Image6); WindowImage.Add(W.Image7);
            WindowImage.Add(W.Image8); WindowImage.Add(W.Image9);

            TextBoxImgDesc.Add(W.TextBoxImage0); TextBoxImgDesc.Add(W.TextBoxImage1); TextBoxImgDesc.Add(W.TextBoxImage2);
            TextBoxImgDesc.Add(W.TextBoxImage3); TextBoxImgDesc.Add(W.TextBoxImage4); TextBoxImgDesc.Add(W.TextBoxImage5);
            TextBoxImgDesc.Add(W.TextBoxImage6); TextBoxImgDesc.Add(W.TextBoxImage7); TextBoxImgDesc.Add(W.TextBoxImage8); TextBoxImgDesc.Add(W.TextBoxImage9);



            W.DefImg = W.Image0.Source.Clone();

        }

        //Сканирование директории поиск всех файлов .jpg показ их на экране
        private void ButtonScanImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string dstcat = W.DstGameDirPath + W.TextBoxCatalogName.Text;

            //получем имена файлов сортируем по алфавиту
            ImageListJpg = Directory.GetFiles(Path.Combine(W.SrcGameDirPath, W.GameCatalogName), "*.jpg").ToList();
            ImageListJpg.Sort((s, s1) => s.CompareTo(s1));

            //отображаем картинки на экране
            for (int i = 0; i < 10; i++)
            {
                //если картинка есть показываем ее
                if (i < ImageListJpg.Count)
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(ImageListJpg[i]);
                    b.EndInit();
                    WindowImage[i].Source = b;
                }
                //иначе показываем зеленный квадрат
                else
                {
                    WindowImage[i].Source = W.DefImg;
                }
            }


            foreach (TextBox textBox in TextBoxImgDesc)
                textBox.Text = W.TextBoxTitleEn.Text;

            W.TextBoxImage0.Text = "Коробка";

            //W.TextBoxImage1.Text = W.TextBoxImage2.Text = W.TextBoxImage3.Text = W.TextBoxImage4.Text = W.TextBoxImage5.Text = W.TextBoxImage6.Text = W.TextBoxImage7.Text = W.TextBoxImage8.Text = W.TextBoxImage9.Text = W.TextBoxTitleEn.Text;

            W.Log.Add($"Найдено {ImageListJpg.Count}  изображений");
        }

        //Создание уменьшеных изображений и изображений с воденным знаком
        private void ButtonConvertImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int WatemarkPosition = WatermarkIndex();
            string dst = W.DstGameDirPath + W.TextBoxCatalogName.Text;

            foreach (string src in ImageListJpg)
            {
                ImageMagik.CreatePNG(src, Path.Combine(dst, Path.ChangeExtension(Path.GetFileName(src), ".png")));
                ImageMagik.JPGConvert(src, Path.Combine(dst, Path.GetFileName(src)));
                ImageMagik.AddWatermark(Path.Combine(dst, Path.GetFileName(src)), Path.Combine(dst, Path.GetFileName(src)), WatemarkPosition);

                W.Log.Add($"Обработано: {src}");

            }

            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            for (int i = 0; i < ImageListJpg.Count; i++)
            {
                GameImage GameImage = new GameImage();
                GameImage.FulllImagePath ="/"+ Path.Combine(W.TextBoxCatalogName.Text, Path.GetFileName(ImageListJpg[i])).Replace('\\', '/');
                GameImage.SmallImagePath = "/" + Path.Combine(W.TextBoxCatalogName.Text, Path.ChangeExtension(Path.GetFileName(ImageListJpg[i]), ".png")).Replace('\\', '/');
                GameImage.DescriptImage = TextBoxImgDesc[i].Text;
                GameImage.GameID = W.GameID;

                GameEntities.GameImage.Add(GameImage);
            }

            try
            {
                GameEntities.SaveChanges();
                W.Log.Add("Изображения добавлены в базу");

                W.GridGameImage.Visibility = Visibility.Hidden;
                W.GridTag.Visibility = Visibility.Visible;

            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
        }



    }
}
