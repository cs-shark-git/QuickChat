using ChatClient.Models;

namespace ChatClient.Services.Client.Parsers
{
    internal class ConnectMessageParser : IMessageParser
    {
        private const string CONNECT_MSG = "UC";

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
