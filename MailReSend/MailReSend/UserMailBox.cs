using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;
using ImapX;
using System.Globalization;
 

namespace MailReSend
{
    public class UserMailBox
    { 
        public MailServer ImapServer;
        public MailServer SmtpServer; 
        ImapClient Imap = null;
        public bool IsErrorGettingMail = false;
        public bool IsErrorSendingMail = false;
        private string msg = "";
        public bool IsWorking = true;
        public List<Message> GettingMessageList = new List<Message>(); 
        public List<KeyWord> KeyWordsList;
        public MessageSetting MailSetting;

        public UserMailBox(MailServer _imapServer, MailServer _smtpServer, MessageSetting _mailReSendSetting, List<KeyWord> _keyWordsList)
        {
            this.ImapServer = _imapServer;
            this.SmtpServer = _smtpServer;
            this.MailSetting = _mailReSendSetting;
            this.KeyWordsList = _keyWordsList;
        } 

        public void GetFilteredMails()
        { 
            try
            { 
                Imap = new ImapClient(ImapServer.Address, ImapServer.Port, true, true);

                Imap.Connect(ImapServer.Address, true, true);
                Imap.Login(ImapServer.ServerUser.Login, ImapServer.ServerUser.Password);
                
                //ImapX.Message[] gettingMailsList = Imap.Folders.Inbox.Search("FROM address personalno.work@ya.ru");
                ImapX.Message[] gettingMailsList = Imap.Folders.Inbox.Search("SINCE " + MailSetting.LastDate.ToString("d-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US")));
                IsErrorGettingMail = false;
                foreach ( var newGetMail in gettingMailsList)
                { 
                    if (!GettingMessageList.Exists(message=> message.GetMail.UId == newGetMail.UId))
                    {   
                        Message newMessage = new Message(newGetMail);
                        newMessage.GettingDateTime = DateTime.Now;

                        if (newMessage.IsFilteredFrom(MailSetting.FromUsers.List))
                        {
                            if (newMessage.FilterToGet(KeyWordsList, MailSetting))
                            {
                            
                                if (newMessage.GetMail.Subject == null)
                                {
                                    newMessage.GetMail.Subject = "UnKnownSubject";
                                }
                                GettingMessageList.Add(newMessage);
                                msg = "Success: Полученно сообщение Uid: " + newMessage.GetMail.UId.ToString() + " От: " + newMessage.GetMail.From.ToString() + " Тема: " + newMessage.GetMail.Subject.ToString() + "дата: " + newMessage.GetMail.Date.ToString();
                                Log.Write(msg);
                            } 
                        }

                         
                    }
                } 
            }
            catch (Exception err)
            {
                msg = "Error in GetFilteredMails: " + err.Message;
                Log.Write(msg);
                IsErrorGettingMail = true;
            }
            finally
            {
                if (Imap != null)
                { 
                    Imap.Disconnect();
                }
            } 
        }

        public void SendFilteredMails()
        {
            try
            {
                for (int i = 1; i <= GettingMessageList.Count; i++)
                {
                    MailSetting.GetRegDataMessageSetting();
                    if (GettingMessageList[GettingMessageList.Count - i].FilterToGet(KeyWordsList, MailSetting))
                    {
                        GettingMessageList[GettingMessageList.Count - i].SetMailSettings(SmtpServer, MailSetting);
                        GettingMessageList[GettingMessageList.Count - i].Send(SmtpServer);
                        GettingMessageList[GettingMessageList.Count - i].SendingDateTime = DateTime.Now;
                        MailSetting.LastDate = GettingMessageList[GettingMessageList.Count - i].GetMail.Date.Value;
                        MailSetting.SetRegDataLastDate();
                        IsErrorSendingMail = false;
                    }
                } 
            }
            catch (Exception err)
            {
                msg = "Error in SendFilteredMails" + err.Message;
                Log.Write(msg);
                IsErrorSendingMail = true;
            }
            //finally
            //{
            //    if (Imap != null)
            //    { 
            //        Imap.Disconnect();
            //    }
            //}
        }

       
    }
}
