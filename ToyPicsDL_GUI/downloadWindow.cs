using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ToyPicsDL_GUI {
    public partial class downloadWindow : Form {

        private Dictionary<string, string> info = new Dictionary<string, string>();
        private BackgroundWorker t = new BackgroundWorker();
        private ToyPics.Class get;

        public downloadWindow(string link, string downloadLoco, Boolean userPage = false) {
            InitializeComponent();
            this.button2.Enabled = false;
            this.info.Add("toDownload", link);
            this.info.Add("saveLoco", downloadLoco);
            this.get = new ToyPics.Class(this.info["toDownload"], this.info["saveLoco"], userPage);
            this.download();
        }

        private void download() {
            t.DoWork += bgDownload;
            t.WorkerSupportsCancellation = true;
            t.RunWorkerAsync();
        }

        private void bgDownload(object sender, DoWorkEventArgs e) {
            get.OnProgressUpdate += Get_OnProgressUpdate;
            get.OnProgressFinished += Get_OnProgressFinished;
            info.Add("videoTitle", get.getTitle());

            this.InvokeIfRequired(() => {
                this.whatsDownloading.Text = this.info["videoTitle"];
            });

            get.downloadVid();
        }

        private void Get_OnProgressFinished(object sender, AsyncCompletedEventArgs e) {
            if (e.Cancelled) {
                File.Delete(String.Concat(this.info["saveLoco"], this.info["videoTitle"], ".mp4"));
            }
            if (!e.Cancelled) {
                this.InvokeIfRequired(() => {
                    this.button2.Enabled = true; //continue button
                    this.button1.Enabled = false; //cancel button
                });
            }
        }

        private void Get_OnProgressUpdate(object sender, System.Net.DownloadProgressChangedEventArgs e) {
            this.InvokeIfRequired(() => {
                this.downloadProgress.Value = e.ProgressPercentage;
                this.label2.Text = String.Concat(e.ProgressPercentage.ToString(),"%");
                this.label1.Text = String.Concat((e.BytesReceived / 131072), "MB / ", (e.TotalBytesToReceive / 131072), "MB");
            });
        }

        //OK/Continue Button
        private void button2_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
        }

        //Cancel Button
        private void button1_Click(object sender, EventArgs e) {
            DialogResult cancelMessage = MessageBox.Show("Are you sure you want to cancel the current download?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (cancelMessage == DialogResult.OK) {
                get.stopDownload();
                t.CancelAsync();
                t.Dispose();
            } else if (cancelMessage == DialogResult.Cancel) {
                return;
            }
            this.DialogResult = DialogResult.Cancel;
        }
    }

    public static partial class Control {
        /// <summary>
        /// invoke wrapper
        /// use like .InvokeIfRequired(() => {obj.Text = asdased}) eta..
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action) {
            if (obj.InvokeRequired) {
                var args = new object[0];
                obj.Invoke(action, args);
            } else {
                action();
            }
        }
    }
}
