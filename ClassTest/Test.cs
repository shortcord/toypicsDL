using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyPics;

namespace Something
{
    class Test
    {
        static void Main()
        {
            using (ToyPics.Class get = new ToyPics.Class("https://videos.toypics.net/view/3300/butt-bouncing-on-chance~/", @"D:\Personal\Desktop\", false))
            {
                get.downloadVid();
            }
        }

    }
}
