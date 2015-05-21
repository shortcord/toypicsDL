using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using CommandLine;
using CommandLine.Text;

namespace ToyPics_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Form1 mainform { get; private set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainform = new Form1();
            //mainform.Font = new Font("Segoe UI", 9);
            mainform.Text = "ToyPics Downloader";
            Application.Run(mainform);
            //Application.Run(new Form1());
        }
    }
}
