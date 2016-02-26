using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CMS20.Controllers
{
    class ParseUrl
    {
        private MainWindow W;

        public ParseUrl(MainWindow w)
        {
            W = w;
            W.ButtonParseUrl.Click += ButtonParseUrl_Click;

        }

        private void ButtonParseUrl_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Under construction");
        }
    }
}
