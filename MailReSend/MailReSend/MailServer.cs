using System;
using System.Collections.Generic;
using System.IO;
//using Limilabs.Client.POP3;
//using Limilabs.Mail;

namespace MailReSend
{
    public class MailServer
    {
        public string Address;
        public int Port;
        public User ServerUser;

        RegMailServer RegMailServer;
          
        public MailServer(string _address, int _port)
        {
            this.Address = _address;
            this.Port = _port;
        }

        public MailServer(string _address, int _port, User _serverUser)
        {
            this.Address = _address;
            this.Port = _port;
            this.ServerUser = _serverUser;
        }

        public MailServer(string _address, int _port, User _serverUser, RegMailServer _regMailServer)
        {
            this.Address = _address;
            this.Port = _port;
            this.ServerUser = _serverUser;
            this.RegMailServer = _regMailServer;
        }

        public MailServer(RegMailServer _regMailServer, User _serverUser)
        {
            this.ServerUser = _serverUser;
            this.RegMailServer = _regMailServer;
        }

        public void GetRegDataMailServer()
        {
            Address = RegMailServer.RegHost.GetValueString().ToString();
            Port = RegMailServer.RegPort.GetValueInt32();
            ServerUser.GetRegDataUser(); 
        }

        public void GetRegDataMailServer(MailServer _defaultValue)
        {
            Address = RegMailServer.RegHost.GetValueString(_defaultValue.Address).ToString();
            Port = RegMailServer.RegPort.GetValueInt32(_defaultValue.Port);
            ServerUser.GetRegDataUser(_defaultValue.ServerUser);
        }

        public void SetRegDataMailServer()
        {
            RegMailServer.RegHost.SetValue(Address);
            RegMailServer.RegPort.SetValue(Port);
            ServerUser.SetRegDataUser();

        }

        public void SetToTextBox()
        {

        }
    }

}
