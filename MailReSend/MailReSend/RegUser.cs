using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class RegUser
    {
        public   Reg RegLogin;
        public Reg RegPassword;

        public RegUser(Reg _regLogin, Reg _regPassword)
        {
            this.RegLogin = _regLogin;
            this.RegPassword = _regPassword;
        }
    }
}
