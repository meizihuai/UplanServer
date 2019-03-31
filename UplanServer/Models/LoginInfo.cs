using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class LoginInfo
    {
        public string usr;
        public string name;
        public string token;
        public int power;
        public int state;
        public LoginInfo()
        {

        }
        public LoginInfo(string usr, string name, string token, int power, int state)
        {
            this.usr = usr;
            this.name = name;
            this.token = token;
            this.power = power;
            this.state = state;
        }
    }
}