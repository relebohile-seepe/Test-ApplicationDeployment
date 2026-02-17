namespace Updater
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblStatus = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            SuspendLayout();
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblStatus.Location = new System.Drawing.Point(250, 150);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(130, 32);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Updating...";
            //
            // progressBar1
            //
            progressBar1.Location = new System.Drawing.Point(150, 210);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(500, 30);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.TabIndex = 1;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(progressBar1);
            Controls.Add(lblStatus);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Updater";
            Shown += new System.EventHandler(Form1_Shown);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
