using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Threading;
using System.Windows.Forms;
using ImapX;
using System.Reflection;
using System.IO;
//using Pop3;

namespace MailReSend
{
    public partial class MailReSendForm : Form
    {
        public string msg = "";
        static public string KeyWordsFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\KeyWords.txt";
        static public Reg RegIsSettingsOpened = new Reg("ReSendMail", "IsSettingOpened"); 
        static public RegUser RegMainUser = new RegUser(new Reg("ReSendMail", "MainUserLogin"), new Reg("ReSendMail", "MainUserPassword")); 
        static public RegMailServer RegImapServer = new RegMailServer(new Reg("ReSendMail", "ImapHost"), new Reg("ReSendMail", "ImapPort"), RegMainUser); 
        static public RegMailServer RegSmtpServer = new RegMailServer(new Reg("ReSendMail", "SmtpHost"), new Reg("ReSendMail", "SmtpPort"), RegMainUser);
        static public RegMessageSetting RegMessageReSendSetting = new RegMessageSetting(new Reg("ReSendMail", "ToUser"), new Reg("ReSendMail", "Subject"), new Reg("ReSendMail", "BodyEnd"), new Reg("ReSendMail", "LastDateTime"), new Reg("ReSendMail", "WorkInterval"), new Reg("ReSendMail", "FromList"));
         
        public User MainUser;
        static User MainUserDefaultData = new User("tresend@mail.ru", "1qaz@WSX%$$%");

        public MailServer ImapServer;
        public MailServer SmtpServer;
        static MailServer ImapServerDefaultData = new MailServer("imap.mail.ru", 143, MainUserDefaultData);
        static MailServer SmtpServerDefaultData = new MailServer("smtp.mail.ru", 587, MainUserDefaultData);


        public MessageSetting MessageReSendSetting; 
        MessageSetting MessageReSendSettingDefaultData = new MessageSetting("tresend@mail.ru", "MainTest", "AlexDreamer Dev", Convert.ToDateTime("2016-01-12 15:20:20"), 5, new FromUsers());
        public bool IsSettingsOpened;
         

        UserMailBox MainUserMailBox;
        View.ListView_UserMailBox ListViewMessages;
        View.ListViewGeniral FromAddress;

        Thread ThreadGetSendShowMails; 
        Thread ThreadStoping;
       
        public bool IsClosed;

        View.ListViewKeyWords ListView_KeyWords;
        KeyWords KeyWords = new KeyWords();
         
        public MailReSendForm()
        {
            InitializeComponent();

            MainUser = new User(RegMainUser);
            MainUser.GetRegDataUser(MainUserDefaultData);

            ImapServer = new MailServer(RegImapServer, MainUser);
            ImapServer.GetRegDataMailServer(ImapServerDefaultData);

            SmtpServer = new MailServer(RegSmtpServer, MainUser);
            SmtpServer.GetRegDataMailServer(SmtpServerDefaultData);

            MessageReSendSetting = new MessageSetting(RegMessageReSendSetting);

            MessageReSendSettingDefaultData.FromUsers.List.Add("nobody@mx3.gazneftetorg.ru");
            MessageReSendSettingDefaultData.FromUsers.List.Add("nobody@mx4.gazneftetorg.ru");
            MessageReSendSettingDefaultData.FromUsers.List.Add("boris.maloi@gmail.com");
            MessageReSendSetting.GetRegDataMessageSetting(MessageReSendSettingDefaultData);

            splitContainer_Main_Filter.Panel2Collapsed = RegIsSettingsOpened.GetValueBool(true); 
        } 
         
        private void buttonOpenSettings_Click(object sender, EventArgs e)
        { 
            splitContainer_Main_Filter.Panel2Collapsed = !splitContainer_Main_Filter.Panel2Collapsed;
            RegIsSettingsOpened.SetValue(splitContainer_Main_Filter.Panel2Collapsed); 
        }

        private void textBox_SMTPServer_TextChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.SmtpServer.Address = textBox_SmtpServer.Text;
            ListViewMessages.MailBox.SmtpServer.SetRegDataMailServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            try
            {
                ListView_KeyWords = new View.ListViewKeyWords(listView_WordsFilter); 
                ListView_KeyWords.KeyWords.ImportFile(KeyWordsFilePath);
                ListView_KeyWords.UpdateListView(); 
               
            }
            catch (Exception error)
            {
                msg = error.Message;
                MessageBox.Show(msg, "Подключение Ключевых слов из файла " + ListView_KeyWords.KeyWords.FilePathDefault, MessageBoxButtons.OK);
            }

            MainUserMailBox = new UserMailBox(ImapServer, SmtpServer, MessageReSendSetting, ListView_KeyWords.KeyWords.List);
            ListViewMessages = new View.ListView_UserMailBox(MainUserMailBox, listView_Log, this,ThreadGetSendShowMails, button_Start);

            MainUserMailBox.MailSetting.FromUsers.ListViewFrom = new View.ListViewGeniral(listView_From);
            MainUserMailBox.MailSetting.FromUsers.UpdateListView();

            //FromAddress.DataList.AddRange(ListViewMessages.MailBox.MailSetting.FromList);
            //FromAddress.UpdateListView();
            


            View.SetTextBox(textBox_Login, MainUser.Login, null);
            View.SetTextBox(textBox_Password, MainUser.Password, null);

            View.SetTextBox(textBox_ImapServer, ImapServer.Address, null);
            View.SetNumeric(numericUpDown_ImapPort, ImapServer.Port, -1);
           

            View.SetTextBox(textBox_SmtpServer, SmtpServer.Address, null);
            View.SetNumeric(numericUpDown_SmtpPort, SmtpServer.Port, -1);

            View.SetTextBox(textBox_MailTo, MessageReSendSetting.UserTo, null);
            View.SetTextBox(textBox_Subject, MessageReSendSetting.Subject, null);
            View.SetTextBox(textBox_BodyEnd, MessageReSendSetting.BodyEnd, null);
            View.SetNumeric(numericUpDown_WorkInterval, MessageReSendSetting.WorkInterval, -1);
            View.SetDateTime(dateTimePicker_DateStartGetting, MessageReSendSetting.LastDate, DateTime.Today);


             
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            button_Start.Enabled = false; 
            ListViewMessages.MailBox.IsWorking = true;

            ThreadGetSendShowMails = new Thread(ListViewMessages.ThreadWork);
            ListViewMessages.ThreadGetSendShowMails = ThreadGetSendShowMails;
            ThreadGetSendShowMails.Start(); 
        }
         
        private void button_Stop_Click(object sender, EventArgs e)
        {
            if (ThreadGetSendShowMails.IsAlive)
            {
                ThreadStoping = new Thread(ListViewMessages.ThreadWorkStop);
                ThreadStoping.Start();
            }
            else
            {
                ListViewMessages.ButtonEnabled(true);
            }
        }

        private void добавитьКлСлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyWordEdit keyWordForm = new KeyWordEdit(null);
            DialogResult res = keyWordForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                string msg = "";
                ListView_KeyWords.KeyWords.Add(new KeyWord(keyWordForm.KeyWord));
                msg = ListView_KeyWords.KeyWords.msg;
                //ListView_KeyWords.KeyWords.ExportFile(ListView_KeyWords.KeyWords.FilePathDefault); 
                ListView_KeyWords.UpdateListView(); 
                MessageBox.Show(msg, "Добавление...", MessageBoxButtons.OK);
            }
        }

        private void загрузитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = openFileDialog1.FileName; 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {  
                    ListView_KeyWords.KeyWords.ImportFile(path);
                    ListView_KeyWords.UpdateListView();
                    msg = "Succes: Загружены Ключевые слова из файла " + path;  
                } 
                catch (Exception error)
                {
                    msg = error.Message;
                }
                MessageBox.Show(msg, "Подключение Ключевых слов из файла " + ListView_KeyWords.KeyWords.FilePathDefault, MessageBoxButtons.OK);
            }
        }

        private void выгрузитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = KeyWords.FilePathDefault;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = "";
                try
                {
                    path = openFileDialog1.FileName;
                    ListView_KeyWords.KeyWords.ExportFile(path);
                    msg = "Succes: Все кл слова выгружены в файл " + path;
                }
                catch (Exception error)
                {
                    msg = error.Message;
                }
                MessageBox.Show(msg, "Выгрузка Ключевых слов в файл ", MessageBoxButtons.OK);
            }

            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    string path = folderBrowserDialog1.SelectedPath;
            //    KeyWords.ExportFile(path + "\\KeyWords_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt"); 
            //} 
        }
         
        private void dateTimePicker_DateStartGetting_ValueChanged(object sender, EventArgs e)
        {
            View.SetRegLastDateDateNumeric(dateTimePicker_DateStartGetting, MessageReSendSetting);
        }

        public void ThreadStoppingWork()
        {
            ListViewMessages.MailBox.IsWorking = false;
            this.Invoke(new Action(() =>
            {
                this.Text = "Остановка потока сбора сообщений!";
            }));
            while (ThreadGetSendShowMails.IsAlive)
            {
                
            }
            if (!ThreadGetSendShowMails.IsAlive)
            { 
                this.Invoke(new Action(() =>
                {
                    this.Text = "Выкл Обработка Почты";
                }));

                button_Start.Invoke(new Action(() =>
                {
                    button_Start.Enabled = true;
                })); 
            } 
        }

        public void ThreadClosing()
        {
            ListViewMessages.ThreadWorkStop();
            this.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }


        private void textBox_Login_TextChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.SmtpServer.ServerUser.RegUser.RegLogin.SetValue(textBox_Login.Text);
        }

        private void textBox_Password_TextChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.SmtpServer.ServerUser.RegUser.RegPassword.SetValue(textBox_Password.Text);
        }

        private void numericUpDown_SmtpPort_ValueChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.SmtpServer.Port = Convert.ToInt32(numericUpDown_SmtpPort.Value);
            ListViewMessages.MailBox.SmtpServer.SetRegDataMailServer();
        }

        private void numericUpDown_ImapPort_ValueChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.ImapServer.Port = Convert.ToInt32(numericUpDown_ImapPort.Value);
            ListViewMessages.MailBox.ImapServer.SetRegDataMailServer();
        }

        private void numericUpDown_SmtpPort_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewMessages.MailBox.SmtpServer.Port = Convert.ToInt32(numericUpDown_SmtpPort.Value);
            ListViewMessages.MailBox.SmtpServer.SetRegDataMailServer();
        }

        private void textBox_BodyEnd_TextChanged(object sender, EventArgs e)
        {
            MessageReSendSetting.BodyEnd = textBox_BodyEnd.Text;
            MessageReSendSetting.SetRegDataMessageSetting();
        } 

        private void textBox_MailTo_TextChanged(object sender, EventArgs e)
        {
            MessageReSendSetting.UserTo = textBox_MailTo.Text;
            MessageReSendSetting.SetRegDataMessageSetting();
        }

        private void textBox_Subject_TextChanged(object sender, EventArgs e)
        {
            MessageReSendSetting.Subject = textBox_Subject.Text;
            MessageReSendSetting.SetRegDataMessageSetting();
        }

        private void textBox_ImapServer_TextChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.ImapServer.Address = textBox_ImapServer.Text;
            ListViewMessages.MailBox.ImapServer.SetRegDataMailServer();
        }
         
        private void contextMenuStrip_KeyWord_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            добавитьКлСлToolStripMenuItem.Visible = true;
            загрузитьФайлToolStripMenuItem.Visible = true;
            выгрузитьВФайлToolStripMenuItem.Visible = true;

            if (ListView_KeyWords.ListView.SelectedItems.Count > 1)
            { 
                удалитьToolStripMenuItem.Visible = false;
                редактироватьToolStripMenuItem.Visible = false;
                
            }
            if (ListView_KeyWords.ListView.SelectedItems.Count == 1)
            { 
                удалитьToolStripMenuItem.Visible = true;
                редактироватьToolStripMenuItem.Visible = true; 
            }
            if (ListView_KeyWords.ListView.SelectedItems.Count < 1)
            { 
                удалитьToolStripMenuItem.Visible = false;
                редактироватьToolStripMenuItem.Visible = false; 
            }
        }

        private void listView_WordsFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            KeyWordEdit keyWordForm = new KeyWordEdit(ListView_KeyWords.ListView.SelectedItems[0].Text);
            DialogResult res = keyWordForm.ShowDialog();

            if (res == DialogResult.OK)
            {
                string msg = "";
                ListView_KeyWords.KeyWords.Edit(ListView_KeyWords.ListView.SelectedItems[0].Text, keyWordForm.KeyWord);
                msg = ListView_KeyWords.KeyWords.msg;
                //ListView_KeyWords.KeyWords.ExportFile(ListView_KeyWords.KeyWords.FilePathDefault); 
                ListView_KeyWords.UpdateListView();
                MessageBox.Show(msg, "Редактирование кл слова", MessageBoxButtons.OK);
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы точно хотите удалить Ключевое слово " + ListView_KeyWords.ListView.SelectedItems[0].Text, "Удаление...", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                string msg = "";
                ListView_KeyWords.KeyWords.Delete(ListView_KeyWords.ListView.SelectedItems[0].Text);
                msg = ListView_KeyWords.KeyWords.msg;
                //ListView_KeyWords.KeyWords.ExportFile(ListView_KeyWords.KeyWords.FilePathDefault);
                ListView_KeyWords.UpdateListView();
                MessageBox.Show(msg, "Удаление кл слова", MessageBoxButtons.OK);
            }
             
        }
         
        private void MailReSendForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThreadGetSendShowMails != null)
            {
                if (ThreadGetSendShowMails.IsAlive)
                {
                    ThreadStoping = new Thread(ThreadClosing);
                    ThreadStoping.Start();
                    e.Cancel = true;
                }
            }
        }

        private void numericUpDown_WorkInterval_ValueChanged(object sender, EventArgs e)
        {
            ListViewMessages.MailBox.MailSetting.WorkInterval = Convert.ToInt32(numericUpDown_WorkInterval.Value);
            ListViewMessages.MailBox.MailSetting.SetRegDataMessageSetting();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox_BodyEndMail_Enter(object sender, EventArgs e)
        {

        }

        private void listView_From_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string chooseFromUser = MainUserMailBox.MailSetting.FromUsers.ListViewFrom.ListView.SelectedItems[0].Text;
            DialogResult res = MessageBox.Show("Вы точно хотите удалить Отправителя слово " + chooseFromUser, "Удаление...", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                string msg = "";
                MainUserMailBox.MailSetting.FromUsers.Delete(chooseFromUser);
                MainUserMailBox.MailSetting.SetRegDataMessageSetting();
                msg = MainUserMailBox.MailSetting.FromUsers.msg;
                MainUserMailBox.MailSetting.FromUsers.UpdateListView();
                MessageBox.Show(msg, "Удаление кл слова", MessageBoxButtons.OK);
            } 
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyWordEdit UserFromForm = new KeyWordEdit(null);
            DialogResult res = UserFromForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                string msg = "";
                MainUserMailBox.MailSetting.FromUsers.Add(UserFromForm.KeyWord);
                MainUserMailBox.MailSetting.SetRegDataMessageSetting();
                msg = MainUserMailBox.MailSetting.FromUsers.msg;
                MainUserMailBox.MailSetting.FromUsers.UpdateListView();
                MessageBox.Show(msg, "Добавление...", MessageBoxButtons.OK);
            }
        }

        private void contextMenuStrip_FromWord_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            добавитьToolStripMenuItem.Visible = true;
            ListView.SelectedListViewItemCollection chooseListViewItems = MainUserMailBox.MailSetting.FromUsers.ListViewFrom.ListView.SelectedItems;
            if (chooseListViewItems.Count > 1)
            {
                удалитьToolStripMenuItem1.Visible = false;
                редактироватьToolStripMenuItem1.Visible = false; 
            }
            if (chooseListViewItems.Count == 1)
            {
                удалитьToolStripMenuItem1.Visible = true;
                редактироватьToolStripMenuItem1.Visible = true;
            }
            if (chooseListViewItems.Count < 1)
            {
                удалитьToolStripMenuItem1.Visible = false;
                редактироватьToolStripMenuItem1.Visible = false;
            }
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string chooseUserFrom = MainUserMailBox.MailSetting.FromUsers.ListViewFrom.ListView.SelectedItems[0].Text;
            KeyWordEdit keyWordForm = new KeyWordEdit(chooseUserFrom);
            DialogResult res = keyWordForm.ShowDialog(); 
            if (res == DialogResult.OK)
            {
                string msg = "";
                MainUserMailBox.MailSetting.FromUsers.Edit(chooseUserFrom, keyWordForm.KeyWord); 
                msg = MainUserMailBox.MailSetting.FromUsers.msg;
                //ListView_KeyWords.KeyWords.ExportFile(ListView_KeyWords.KeyWords.FilePathDefault); 
                MainUserMailBox.MailSetting.FromUsers.UpdateListView();
                MessageBox.Show(msg, "Редактирование кл слова", MessageBoxButtons.OK);
            }
        }

        private void checkBox_IsPasswordShow_CheckedChanged(object sender, EventArgs e)
        {

            textBox_Password.UseSystemPasswordChar= checkBox_IsPasswordShow.Checked ? true : false; 
        }
    }
}
