using ChatClient.Models;
using ChatClient.Services.Client.Parsers.Base;

namespace ChatClient.Services.Client.Parsers
{
    internal class DisconnectMessageParser : IMessageParser
    {
        private const string DISCONNECT_MSG = "UD";

        public bool Parse(Message msg)
        {
            if(msg.Text.Substring(0, 2) == DISCONNECT_MSG)
            {
                return true;
            }
            return false;
        }
    }
}
