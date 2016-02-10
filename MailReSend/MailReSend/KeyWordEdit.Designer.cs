namespace MailReSend
{
    partial class KeyWordEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_KeyWord = new System.Windows.Forms.GroupBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.textBox_KeyWord = new System.Windows.Forms.TextBox();
            this.groupBox_KeyWord.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_KeyWord
            // 
            this.groupBox_KeyWord.Controls.Add(this.textBox_KeyWord);
            this.groupBox_KeyWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_KeyWord.Location = new System.Drawing.Point(0, 0);
            this.groupBox_KeyWord.Name = "groupBox_KeyWord";
            this.groupBox_KeyWord.Size = new System.Drawing.Size(204, 41);
            this.groupBox_KeyWord.TabIndex = 0;
            this.groupBox_KeyWord.TabStop = false;
            this.groupBox_KeyWord.Text = "Ключевое слово";
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_OK.Location = new System.Drawing.Point(0, 41);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(204, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // textBox_KeyWord
            // 
            this.textBox_KeyWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_KeyWord.Location = new System.Drawing.Point(3, 16);
            this.textBox_KeyWord.Name = "textBox_KeyWord";
            this.textBox_KeyWord.Size = new System.Drawing.Size(198, 20);
            this.textBox_KeyWord.TabIndex = 0;
            // 
            // KeyWordEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 64);
            this.Controls.Add(this.groupBox_KeyWord);
            this.Controls.Add(this.button_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyWordEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.KeyWordEdit_Load);
            this.groupBox_KeyWord.ResumeLayout(false);
            this.groupBox_KeyWord.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_KeyWord;
        private System.Windows.Forms.TextBox textBox_KeyWord;
        private System.Windows.Forms.Button button_OK;
    }
}