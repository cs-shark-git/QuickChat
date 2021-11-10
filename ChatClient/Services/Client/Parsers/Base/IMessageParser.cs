using ChatClient.Models;

namespace ChatClient.Services.Client.Parsers.Base
{
    interface IMessageParser
    {
        bool Parse(Message msg);
    }
}
