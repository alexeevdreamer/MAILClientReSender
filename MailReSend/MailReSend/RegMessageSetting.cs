using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class RegMessageSetting
    {
        public Reg RegLastDate;
        public Reg RegUserTo;
        public Reg RegSubject;
        public Reg RegEndBody;
        public Reg RegWorkInterval;
        public Reg RegFromList;

        public RegMessageSetting(Reg _regUserTo, Reg _regSubject, Reg _regEndBody, Reg _regLastDate, Reg _regWorkInterval)
        {
            this.RegUserTo = _regUserTo;
            this.RegSubject = _regSubject;
            this.RegEndBody = _regEndBody;
            this.RegLastDate = _regLastDate;
            this.RegWorkInterval = _regWorkInterval;
        }

        public RegMessageSetting(Reg _regUserTo, Reg _regSubject, Reg _regEndBody, Reg _regLastDate, Reg _regWorkInterval, Reg _regFromList)
        {
            this.RegUserTo = _regUserTo;
            this.RegSubject = _regSubject;
            this.RegEndBody = _regEndBody;
            this.RegLastDate = _regLastDate;
            this.RegWorkInterval = _regWorkInterval;
            this.RegFromList = _regFromList;
        }

    }
}
