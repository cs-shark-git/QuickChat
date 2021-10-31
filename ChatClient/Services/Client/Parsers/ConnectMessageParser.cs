using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Models;
using ChatClient.Services.Client.Parsers.Base;

namespace ChatClient.Services.Client.Parsers
{
    class ConnectMessageParser : IMessageParser
    {
        private string CONNECT_MSG = "UC";
        private string _message;

        public bool Parse(Message msg)
        {
            _message = msg.Text;
            if(msg.Text.Substring(0, 2) == CONNECT_MSG)
            {
                return true;
            }
            return false;
        }
       
    }
}
