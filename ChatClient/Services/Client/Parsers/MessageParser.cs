using ChatClient.Models;
using ChatClient.Services.Client.Parsers.Base;

namespace ChatClient.Services.Client.Parsers
{
    class MessageParser
    {
        private IMessageParser _parser;

        public MessageParser(IMessageParser parser)
        {
            _parser = parser;
        }

        public bool Parse(Message msg)
        {
            return _parser.Parse(msg);
        }
    }
}
