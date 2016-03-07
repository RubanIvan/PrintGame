using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CMS20.Controllers
{
    class TagManage
    {

        /// <summary>Ссылка на основное окно</summary>
        private MainWindow W;

        /// <summary>список тэгов</summary>
        List<Tag> TagList = new List<Tag>();

        public TagManage(MainWindow w)
        {
            W = w;
            W.ListBoxTag.Loaded += (sender, args) => { TagRefresh(); };
            W.ButtonAddTag.Click += ButtonAddTag_Click;
            W.ButtonAdd.Click += ButtonAdd_Click;
            W.ButtonRemove.Click += ButtonRemove_Click;
            W.ButtonSaveTagToGame.Click += ButtonSaveTagToGame_Click;
        }



        //удалить тег у игры
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (W.ListBoxGameTag.SelectedIndex < 0) return;
            W.ListBoxGameTag.Items.RemoveAt(W.ListBoxGameTag.SelectedIndex);
        }

        //Добавит тэг к игре
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (W.ListBoxTag.SelectedIndex < 0) return;
            Tag t = (Tag)((ListBoxItem)(W.ListBoxTag.Items[W.ListBoxTag.SelectedIndex])).Tag;
            W.ListBoxGameTag.Items.Add(new ListBoxItem() { Tag = t, Content = t.TagName });

        }

        //добавить тэг к списку тегов
        private void ButtonAddTag_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            GameEntities.Tag.Add(new Tag() { TagName = W.TextBoxAddTag.Text });
            try
            {
                GameEntities.SaveChanges();
                W.Log.Add($"Тэг {W.TextBoxAddTag.Text} добавлен");
                W.TextBoxAddTag.Text = "";
            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
            TagRefresh();
        }

        //Записать теги перейти к файловым архивам
        private void ButtonSaveTagToGame_Click(object sender, RoutedEventArgs e)
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();

            foreach (ListBoxItem item in W.ListBoxGameTag.Items)
            {
                GameEntities.GameTag.Add(new GameTag() { GameID = W.GameID, TagID = ((Tag)(item.Tag)).TagID });
            }
            try
            {
                GameEntities.SaveChanges();
                W.Log.Add("Тэги добавлены");

                W.GridTag.Visibility = Visibility.Hidden;
                W.GridFileManage.Visibility = Visibility.Visible;


            }
            catch (Exception sql)
            {
                MessageBox.Show(sql.Message, "Error", MessageBoxButton.OK);
                return;
            }
        }

        // обновить список тегов
        private void TagRefresh()
        {
            PrintGameDataEntities GameEntities = new PrintGameDataEntities();
            TagList = GameEntities.Tag.ToList();

            W.ListBoxTag.Items.Clear();
            foreach (Tag tag in TagList)
            {
                W.ListBoxTag.Items.Add(new ListBoxItem() { Tag = tag, Content = tag.TagName });
            }

        }

    }
}
