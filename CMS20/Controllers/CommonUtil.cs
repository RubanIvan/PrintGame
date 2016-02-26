using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMS20.Controllers
{
    public static class CommonUtil
    {
        public static void Add(this TextBox box, string Msg)
        {
            box.Text += Msg + "\n";
        }
    }
}
