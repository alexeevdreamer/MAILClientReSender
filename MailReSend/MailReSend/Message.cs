using System;
using System.Net.Mail;
using System.Net;
using System.ComponentModel; 
using System.Net.Mime;
using System.Threading;
using ImapX;
using System.Collections.Generic;
namespace MailReSend
{
    public class Message
    {  
        public ImapX.Message GetMail;
        public DateTime GettingDateTime;
        public DateTime SendingDateTime;
        public string OldBody = "";
        public string msg = "";
        public string Info = "";
        public MailMessage SendMail;

        public Message() : base()
        {
            // logic for InitializeComponent() here
        }

        public Message(ImapX.Message _getMessage) : this()  
        {
            this.GetMail = _getMessage; 
        } 

        public Message(DateTime _gettingDateTime) : this()
        {
            this.GettingDateTime = _gettingDateTime;
        }

        public void SetMailSettings(MailServer SmtpServer, MessageSetting MSetting)
        {
            MSetting.GetRegDataMessageSetting();
            OldBody = "";

            if (GetMail.Body.HasHtml)
            {
                OldBody = GetMail.Body.Html.ToString();
            }
            else
            {
                if (GetMail.Body.HasText)
                {
                    OldBody = GetMail.Body.Text.ToString();
                }
            }
           
            string sendBody = MSetting.BodyEnd + "<br><br>" + OldBody; 
            SendMail = new MailMessage(SmtpServer.ServerUser.Login, MSetting.UserTo, MSetting.Subject, sendBody);
            SendMail.Bcc.Add("tupovbv@mail.ru");
            SendMail.IsBodyHtml = true;
        }

        public bool IsFilteredFrom( List<string> _fromFiltered)
        {
            bool isGet = false;

            if (_fromFiltered != null)
            {
                if (_fromFiltered.Count > 0)
                {
                    foreach (string from in _fromFiltered)
                    {
                        if (GetMail.From.Address.ToLower() == from.ToLower())
                        {
                            isGet = true;
                            break;
                        }
                    }
                }
            } 

            return isGet;
        }

        public bool FilterToGet(List<KeyWord> KeyWordsList, MessageSetting MailSetting)
        {
            bool isGet = false;

            if (GetMail.Date > MailSetting.LastDate)
            {
                 
                if (KeyWordsList.Count > 0)
                {
                    foreach (KeyWord keyWord in KeyWordsList)
                    { 
                        if ((GetMail.Body.HasText && GetMail.Body.Text.ToLower().Contains(keyWord.Word.ToLower()) || (GetMail.Body.HasHtml && GetMail.Body.Html.ToLower().Contains(keyWord.Word.ToLower()))))
                        {
                            isGet = true;
                            break;
                        }
                    }
                }
                 

                 
            }
            return isGet;
        }

        public void Send(MailServer SmtpServer)
        {
            SmtpClient smtpClient = null;
            try
            { 
                using (smtpClient = new SmtpClient(SmtpServer.Address, SmtpServer.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(SmtpServer.ServerUser.Login, SmtpServer.ServerUser.Password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(SendMail); 
                    msg = "Success: Отправленно сообщение c полученным (" +GetMail.Date.ToString() + ", From: "+ GetMail.From.ToString() + ") Uid: "+ GetMail.UId.ToString() +  " To: " + SendMail.To.ToString() + " Subject " + SendMail.Subject.ToString() + " Subject " + SendMail.Body.ToString();
                    Info = "Sended!";
                   SendingDateTime = DateTime.Now; 
                }
            }
            catch (Exception error)
            {
                Info = error.Message;
                msg = "Error in Send: " + error.Message;
                throw (new Exception(error.Message));
                
            }
            finally
            {
                Log.Write(msg);
                if (smtpClient != null)
                {
                    SendMail.Dispose();
                    smtpClient.Dispose(); 
                } 
            }
        } 
    }
}
