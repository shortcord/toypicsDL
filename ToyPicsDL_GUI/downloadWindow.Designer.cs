namespace ToyPicsDL_GUI {
    partial class downloadWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.whatsDownloading = new System.Windows.Forms.Label();
            this.downloadProgress = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // whatsDownloading
            // 
            this.whatsDownloading.Location = new System.Drawing.Point(12, 9);
            this.whatsDownloading.Name = "whatsDownloading";
            this.whatsDownloading.Size = new System.Drawing.Size(199, 23);
            this.whatsDownloading.TabIndex = 0;
            this.whatsDownloading.Text = "Loading...";
            // 
            // downloadProgress
            // 
            this.downloadProgress.Location = new System.Drawing.Point(15, 95);
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.Size = new System.Drawing.Size(857, 23);
            this.downloadProgress.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(797, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(716, 126);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Continue";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 28);
            this.label1.TabIndex = 4;
            this.label1.Text = string.Empty;
            this.label1.AutoSize = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(772, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = string.Empty;
            this.label2.AutoSize = true;
            // 
            // downloadWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 161);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.downloadProgress);
            this.Controls.Add(this.whatsDownloading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 200);
            this.Name = "downloadWindow";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Downloading...";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label whatsDownloading;
        public System.Windows.Forms.ProgressBar downloadProgress;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}