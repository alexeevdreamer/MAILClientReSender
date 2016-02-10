using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class MessageSetting
    {
        public string UserTo = "";
        public string Subject = "";
        public string BodyEnd = "";
        public DateTime LastDate;
        public int WorkInterval;
        public FromUsers FromUsers = new FromUsers(); 
        //public View.ListViewGeniral ListViewFrom;

        RegMessageSetting RegMessageSetting;
         
        public MessageSetting(RegMessageSetting _regMessageSetting)
        { 
            this.RegMessageSetting = _regMessageSetting; 
        } 

        public MessageSetting(string _userTo, string _subject, string _endBody, DateTime _lastDate, int _workInterval, FromUsers _fromUsers)
        { 
            this.UserTo = _userTo;
            this.Subject = _subject;
            this.BodyEnd = _endBody;
            this.LastDate = _lastDate;
            this.WorkInterval = _workInterval;
            this.FromUsers = _fromUsers;
        }

        public void GetRegDataMessageSetting()
        {
            if (RegMessageSetting.RegUserTo.GetValueString().ToString() != null)
            {
                UserTo = RegMessageSetting.RegUserTo.GetValueString().ToString();
            }
            if (RegMessageSetting.RegSubject.GetValueString().ToString() != null)
            {
                Subject = RegMessageSetting.RegSubject.GetValueString().ToString();
            }

            if (RegMessageSetting.RegEndBody.GetValueString().ToString() != null)
            {
                BodyEnd = RegMessageSetting.RegEndBody.GetValueString().ToString();
            }
            LastDate = RegMessageSetting.RegLastDate.GetValueDateTime();
            WorkInterval = RegMessageSetting.RegWorkInterval.GetValueInt32();


            FromUsers.AddList(RegMessageSetting.RegFromList.GetValueStringList());
            
        } 

        public void GetRegDataMessageSetting(MessageSetting _defaultValue)
        {
            if (RegMessageSetting.RegUserTo.GetValueString().ToString() != null)
            {
                UserTo = RegMessageSetting.RegUserTo.GetValueString(_defaultValue.UserTo).ToString();
            }
            if (RegMessageSetting.RegSubject.GetValueString().ToString() != null)
            {
                Subject = RegMessageSetting.RegSubject.GetValueString(_defaultValue.Subject).ToString();
            }

            if (RegMessageSetting.RegEndBody.GetValueString().ToString() != null)
            {
                BodyEnd = RegMessageSetting.RegEndBody.GetValueString(_defaultValue.BodyEnd).ToString();
            }
            LastDate = RegMessageSetting.RegLastDate.GetValueDateTime(_defaultValue.LastDate);
            WorkInterval = RegMessageSetting.RegWorkInterval.GetValueInt32(_defaultValue.WorkInterval);
            FromUsers.AddList(RegMessageSetting.RegFromList.GetValueStringList(_defaultValue.FromUsers.List));
             
        }
         
        public void SetRegDataMessageSetting()
        {
            RegMessageSetting.RegUserTo.SetValue(UserTo);
            RegMessageSetting.RegSubject.SetValue(Subject);
            RegMessageSetting.RegEndBody.SetValue(BodyEnd);
            RegMessageSetting.RegLastDate.SetValue(LastDate);
            RegMessageSetting.RegWorkInterval.SetValue(WorkInterval);
            RegMessageSetting.RegFromList.SetValueStringList(FromUsers.List);
        }

        public void SetRegDataLastDate()
        { 
            RegMessageSetting.RegLastDate.SetValue(LastDate);
        } 
    }
}
