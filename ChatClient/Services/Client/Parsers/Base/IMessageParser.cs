using ChatClient.Models;

namespace ChatClient.Services.Client.Parsers
{
    interface IMessageParser
    {
        bool Parse(Message msg);
    }
}
