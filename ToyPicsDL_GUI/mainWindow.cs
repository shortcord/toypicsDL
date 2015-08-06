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
            this.downloadButton.Enabled = false;
            this.GtGLabel.Text = "Enter a link to download";

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
            //if (this.UriToDownload.BackColor == colors.errorRed)
            //    this.UriToDownload.BackColor = System.Drawing.SystemColors.Window;
            //https://videos.toypics.net/view/3300/butt-bouncing-on-chance~/
            //https://videos.toypics.net/Junee/public/
            string downloadLoco = Properties.Settings.Default.saveLocation;
            string toDownload = this.UriToDownload.Text;
            try {
                if (Regex.IsMatch(this.UriToDownload.Text, "(?i)((videos.toypics.net))")) {
                    if (Regex.IsMatch(toDownload, @"(?i)(/public/)")) { // if it is a user page
                        //userpage = true;
                        //this.button1.Enabled = true;
                        this.downloadButton.Enabled = false;
                        this.GtGLabel.ForeColor = colors.warningFont;
                        this.UriToDownload.BackColor = colors.warningBackground;
                        this.GtGLabel.Text = "Downloading of userpages isn\'t currently implemented";
                    } else if (Regex.IsMatch(toDownload, @"(?i)(/view/)")) { // if it is a video page
                        this.downloadButton.Enabled = true;
                        this.UriToDownload.BackColor = colors.goodBackground;
                        this.GtGLabel.ForeColor = colors.goodFont;
                        this.GtGLabel.Text = "Vaild Link";
                    } else {
                        this.downloadButton.Enabled = false;
                        this.UriToDownload.BackColor = colors.errorBackground;
                        this.GtGLabel.ForeColor = colors.errorFont;
                        this.GtGLabel.Text = "Not a vaild ToyPics link";
                    }
                } else if (string.IsNullOrWhiteSpace(toDownload)) {
                    this.downloadButton.Enabled = false;
                    this.UriToDownload.BackColor = System.Drawing.SystemColors.Window;
                    this.GtGLabel.ForeColor = System.Drawing.SystemColors.ControlText;
                    this.GtGLabel.Text = "Enter a link to download";
                } else {
                    this.downloadButton.Enabled = false;
                    this.UriToDownload.BackColor = colors.errorBackground;
                    this.GtGLabel.ForeColor = colors.errorFont;
                    this.GtGLabel.Text = "Not a vaild ToyPics link";
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            string toDownload = this.UriToDownload.Text;
            try {
                downloadWindow dlWindow = new downloadWindow(toDownload, downloadLoco, userpage);
                DialogResult dlWindowResult = dlWindow.ShowDialog();

                if (dlWindowResult == DialogResult.OK) {
                    countDownloaded++;
                    history.Add(toDownload);
                    this.UriToDownload.Clear();
                }
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
