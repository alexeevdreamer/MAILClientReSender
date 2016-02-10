using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailReSend
{
    public partial class KeyWordEdit : Form
    {
        public string KeyWord = "";

        public KeyWordEdit(string _keyWord)
        {
            if (_keyWord != null)
            {
                this.KeyWord = _keyWord;
            } 
            InitializeComponent();
        }

        private void KeyWordEdit_Load(object sender, EventArgs e)
        {
            textBox_KeyWord.Text = KeyWord;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            KeyWord = textBox_KeyWord.Text;
            this.DialogResult = DialogResult.OK; 
            this.Close();
        }
    }
}
