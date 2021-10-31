using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.Client.Parsers.Base
{
    interface IMessageParser
    {
        bool Parse(Message msg);
    }
}
