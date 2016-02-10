using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace MailReSend
{
    public class View
    {
        static public void GetRegLastDateDateNumeric(DateTimePicker _dateTimePickerLastDateStart, Reg _regLastDate)
        {
            try
            { 
                _dateTimePickerLastDateStart.Value = _regLastDate.GetValueDateTime();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        static public void SetRegLastDateDateNumeric(DateTimePicker _dateTimePickerLastDateStart, MessageSetting _mSetting)
        {
            try
            {
                _mSetting.LastDate = _dateTimePickerLastDateStart.Value;
                _mSetting.SetRegDataLastDate();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        static public void SetTextBox(TextBox _textBox,string _value, string _defaultValue)
        {
            if (_value != "" &&  _value != null)
            {
                _textBox.Text = _value.ToString();
            }
            else
            { 
                if (_defaultValue != null)
                {
                    _textBox.Text = _defaultValue;
                }
                else
                {
                    _textBox.Text = "";
                }
               
            }
            
        }

        static public void SetNumeric(NumericUpDown _numValue, int _value, int _defaultValue)
        {
            try
            {
                if (_value > 0)
                {
                    _numValue.Value = _value;
                }
                else
                {
                    if (_defaultValue  > 0)
                    {
                        _numValue.Value = _defaultValue;
                    }
                    else
                    {
                        _numValue.Value = -1;
                    }

                }
            }
            catch(Exception error)
            {
                Log.Write(error.Message);
            }
           

        }

        static public void SetDateTime(DateTimePicker _dateTime, DateTime _value, DateTime _defaultValue)
        {
            try
            {
                if (_value != null)
                {
                    _dateTime.Value = _value;
                }
                else
                {
                    if (_defaultValue != null)
                    {
                        _dateTime.Value = _defaultValue;
                    }
                    else
                    {
                        _dateTime.Value = DateTime.Today.AddDays(-1);
                    }

                }
            }
            catch (Exception error)
            {
                Log.Write(error.Message);
                _dateTime.Value = DateTime.Now;
            }

        }
        public class ListView_UserMailBox
        {
            public UserMailBox MailBox;
            public ListView ListView;
            public Form MainForm;
            public Thread ThreadGetSendShowMails;
            public Button Button_Start;

            public ListView_UserMailBox() : base()
            { 
            }

            public ListView_UserMailBox(UserMailBox _mailBox) : this ()
            {
                this.MailBox = _mailBox;
            } 
            public ListView_UserMailBox(UserMailBox _mailBox, ListView _listVIew) 
            {
                this.ListView = _listVIew;
                this.MailBox = _mailBox;
            }
            public ListView_UserMailBox(UserMailBox _mailBox, ListView _listVIew, Form _form)
            {
                this.ListView = _listVIew;
                this.MailBox = _mailBox;
                this.MainForm = _form;
            }

            public ListView_UserMailBox(UserMailBox _mailBox, ListView _listVIew, Form _form, Thread _threadGetSendShow)
            {
                this.ListView = _listVIew;
                this.MailBox = _mailBox;
                this.MainForm = _form;
                this.ThreadGetSendShowMails = _threadGetSendShow; 
            }
            public ListView_UserMailBox(UserMailBox _mailBox, ListView _listVIew, Form _form, Thread _threadGetSendShow, Button _button_Start)
            {
                this.ListView = _listVIew;
                this.MailBox = _mailBox;
                this.MainForm = _form;
                this.ThreadGetSendShowMails = _threadGetSendShow;
                this.Button_Start = _button_Start;
            }

            public void ThreadWork()
            {
                
                while (MailBox.IsWorking)
                { 
                    MailBox.ImapServer.GetRegDataMailServer();
                    MailBox.SmtpServer.GetRegDataMailServer();

                    MailBox.MailSetting.GetRegDataMessageSetting();
                     
                    FormName("");

                    AddFormName("Вкл. Обработка почты... " + MailBox.ImapServer.ServerUser.Login);
                   
                    
                    MailBox.GetFilteredMails();
                    if (MailBox.IsErrorGettingMail)
                    {
                        AddFormName(" Ошибка при получении сообщений "); 
                    }
                    MailBox.MailSetting.GetRegDataMessageSetting();
                     
                    MailBox.SendFilteredMails();
                    if (MailBox.IsErrorSendingMail)
                    {
                        AddFormName("Ошибка при отправке сообщений"); 
                    }

                    UpdateListView();
                    ListViewColumnsSetWidth(-2);

                    MailBox.MailSetting.GetRegDataMessageSetting();
                    Thread.Sleep(MailBox.MailSetting.WorkInterval*60000);
                }
            }

            public void ThreadWorkStop()
            {
                MailBox.IsWorking = false;
                AddFormName("Остановка потока сбора сообщений!");
                 
                while (ThreadGetSendShowMails.IsAlive)
                {

                }
                if (!ThreadGetSendShowMails.IsAlive)
                {
                    FormName("Выкл Обработка Почты " + MailBox.ImapServer.ServerUser.Login); 
                    ButtonEnabled(true);
                }
            }

            public void FormName(string _text)
            {
                MainForm.Invoke(new Action(() =>
                {
                    MainForm.Text = _text;
                }));
            }

            public void AddFormName(string _text)
            {
                MainForm.Invoke(new Action(() =>
                {
                    MainForm.Text = MainForm.Text + " " + _text;
                }));
            }

            public void ButtonEnabled(bool _value)
            {
                Button_Start.Invoke(new Action(() =>
                {
                    Button_Start.Enabled = _value;
                }));
            }

            public void UpdateListView()
            { 
                ListView.Invoke(new Action(() =>
                {
                    ListView.Items.Clear();
                    ListView.BeginUpdate();
                    foreach (Message message in MailBox.GettingMessageList)
                    {
                        Add(message);
                    }
                    for (int i = 0; i < ListView.Columns.Count; i++)
                    {
                        ListView.Columns[i].Width = -2;
                    }
                    ListView.EndUpdate();
                }));

               
            }

            public void ListViewColumnsSetWidth(int _width)
            {
                ListView.Invoke(new Action(() =>
                { 
                    ListView.BeginUpdate(); 
                    for (int i = 0; i < ListView.Columns.Count; i++)
                    {
                        ListView.Columns[i].Width = _width;
                    }
                    ListView.EndUpdate();
                }));


            } 

            public void Add(Message _message)
            {
                ListView.Invoke(new Action(() =>
                {
                    ListViewItem item = ListView.Items.Add(_message.GetMail.UId.ToString());
                    item.Tag = _message;
                    item.SubItems.Add(_message.GetMail.Date.ToString());
                    item.SubItems.Add(_message.GetMail.From.ToString());
                    item.SubItems.Add(_message.GetMail.Subject.ToString()); 
                    item.SubItems.Add(_message.OldBody.ToString());
                    item.SubItems.Add(_message.GettingDateTime.ToString());
                    item.SubItems.Add(_message.SendingDateTime.ToString());
                    item.SubItems.Add(_message.Info.ToString());
                }));
            }
        }
        public class ListViewKeyWords
        {
            public KeyWords KeyWords = new KeyWords();
            public ListView ListView;

            public ListViewKeyWords(ListView _listView)
            {
                this.ListView = _listView;
            }
             
            public void UpdateListView()
            {
                ListView.Invoke(new Action(() =>
                {
                     ListView.Items.Clear();
                    ListView.BeginUpdate();
                    foreach (KeyWord keyWord in KeyWords.List)
                    {
                        ListView.Items.Add(keyWord.Word); 
                    }
                    ListView.EndUpdate();
                }));
                
            }

            public void AddKeyWord(KeyWord _keyWord)
            {
                ListViewItem item = ListView.Items.Add(_keyWord.Word);
            }

        }

        public class ListViewGeniral
        { 
            public ListView ListView; 

            public ListViewGeniral(ListView _listView)
            {
                this.ListView = _listView;
            }

            public void UpdateListView(List<string> _dataList)
            {
                ListView.Invoke(new Action(() =>
                {
                    ListView.Items.Clear();
                    ListView.BeginUpdate();
                    foreach (string value in _dataList)
                    {
                        ListView.Items.Add(value.ToString());
                    }
                    ListView.EndUpdate();
                }));

            }

            public void ListViewColumnsSetWidth(int _width)
            {
                ListView.Invoke(new Action(() =>
                {
                    ListView.BeginUpdate();
                    for (int i = 0; i < ListView.Columns.Count; i++)
                    {
                        ListView.Columns[i].Width = _width;
                    }
                    ListView.EndUpdate();
                }));


            }

            public void AddKeyWord(object _value)
            {
                ListViewItem item = ListView.Items.Add(_value.ToString());
            }

        }
    }
}
