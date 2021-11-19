using ChatClient.Models;

namespace ChatClient.Services.Client.Parsers
{
    internal class MessageParser
    {
        private readonly IMessageParser _parser;

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
