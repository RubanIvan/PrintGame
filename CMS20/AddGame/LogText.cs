using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace CMS20.Controllers
{
    /// <summary> 
    /// </summary>
    public class LogText
    {
        private MainWindow W;

        public LogText(MainWindow w)
        {
            W = w;
            W.Log.TextChanged += Log_TextChanged;
           
        }

        //Прокручиваем текст в конец
        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            W.Log.ScrollToEnd();
        }
    }

    
}
