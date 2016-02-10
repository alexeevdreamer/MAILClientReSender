using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class User
    {
        public string Login;
        public string Password;
        public RegUser RegUser;
         
        public User(RegUser _regUser)  
        { 
            this.RegUser = _regUser;
        }

        public User(string _login, string _password) 
        {
            this.Login = _login;
            this.Password = _password;
        }
         
        public void GetRegDataUser()
        {
            Login = RegUser.RegLogin.GetValueString().ToString();
            Password = RegUser.RegPassword.GetValueString().ToString();
        }
        public void GetRegDataUser(User DefaultData)
        { 
            Login = RegUser.RegLogin.GetValueString(DefaultData.Login).ToString();
            Password = RegUser.RegPassword.GetValueString(DefaultData.Password).ToString();
        } 
        public void SetRegDataUser()
        {
            RegUser.RegLogin.SetValue(Login);
            RegUser.RegPassword.SetValue(Password); 
        }


    }
}
