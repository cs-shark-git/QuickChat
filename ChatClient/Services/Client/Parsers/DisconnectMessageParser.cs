using ChatClient.Models;
using ChatClient.Services.Client.Parsers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.Client.Parsers
{
    class DisconnectMessageParser : IMessageParser
    {
        private string CONNECT_MSG = "UD";

        public bool Parse(Message msg)
        {
            if(msg.Text.Substring(0, 2) == CONNECT_MSG)
            {
                return true;
            }
            return false;
        }
    }
}
