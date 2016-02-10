using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class RegMailServer
    {
        public Reg RegHost;
        public Reg RegPort;
        public RegUser RegUser;
         

        public RegMailServer(Reg _regHost,Reg _regPort, RegUser _regUser)
        {
            this.RegHost = _regHost;
            this.RegPort = _regPort;
            this.RegUser = _regUser;
        } 
    }
}
