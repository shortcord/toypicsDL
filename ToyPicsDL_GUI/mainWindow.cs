using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ToyPicsDL_GUI {
    public partial class mainWindow : Form {

        private static ulong countDownloaded = Properties.Settings.Default.timesDownloaded;
        private StringCollection history = Properties.Settings.Default.history;
        private Boolean userpage = false;

        public mainWindow() {
            InitializeComponent();

            if (history == null) // if no history exist then create a new StringCollection
                history = new StringCollection();

            this.FormClosing += MainWindow_FormClosing; //subscribe to Formclosing event

            this.classVersion.Text = String.Concat("v",ToyPics.Class.version); //I would put this in the mainWindow.Designer.cs but it keeps getting rewriten as a string

            if (Properties.Settings.Default.saveLocation != String.Empty) {
                this.whereToSaveTo.Text = Properties.Settings.Default.saveLocation;
            }
        }

        //called when the WinForm is closing
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.history = history;
            Properties.Settings.Default.Save();
        }

        private void UriToDownload_TextChanged(object sender, EventArgs e) {
            if (this.UriToDownload.BackColor == colors.errorRed)
                this.UriToDownload.BackColor = System.Drawing.SystemColors.Window;
        }

        private void button2_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            {
                if (result == DialogResult.OK) { //only write to config if new folder is selected
                    Properties.Settings.Default.saveLocation = String.Concat(fbd.SelectedPath, "\\"); //append \\ to the end of user selected path
                    Properties.Settings.Default.Save();
                }
            }
            this.whereToSaveTo.Text = Properties.Settings.Default.saveLocation;
        }

        //Download Button
        private void button1_Click(object sender, EventArgs e) {

            string downloadLoco = Properties.Settings.Default.saveLocation;
            string toDownload = string.Empty;
            try {
                if (this.UriToDownload.Text != String.Empty &&
               Regex.IsMatch(this.UriToDownload.Text, "(?i)((videos.toypics.net))")) {
                    toDownload = this.UriToDownload.Text;
                } else {
                    MessageBox.Show("The Url you entered isn't correct", "Oops?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.UriToDownload.BackColor = colors.errorRed;
                    return;
                }

                if (Regex.IsMatch(toDownload, @"(?i)(/public/)")) {
                    //userpage = true;
                    throw new NotImplementedException("Downloading of userpages isn\'t currently implemented");
                }

                downloadWindow dlWindow = new downloadWindow(toDownload, downloadLoco, userpage);
                DialogResult dlWindowResult = dlWindow.ShowDialog();

                if (dlWindowResult == DialogResult.OK) {
                    countDownloaded++;
                    history.Add(toDownload);
                    this.UriToDownload.Clear();
                }
            } catch (NotImplementedException ex) {
                MessageBox.Show(ex.Message, "Implementation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch (Exception ex) {
                MessageBox.Show(String.Concat(ex.Message, Environment.NewLine, "The program will now close"), "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            var nl = Environment.NewLine;
            string message = String.Concat("ToyPics Downloader", nl, "ToyPics Class Version: ", ToyPics.Class.version, nl, "ToyPics GUI Verison: ", 0.1, nl, "shortcord.com/github");
            MessageBox.Show(message, "About", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}
