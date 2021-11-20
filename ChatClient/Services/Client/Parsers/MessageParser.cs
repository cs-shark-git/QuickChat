using ChatClient.Models;
using System.Text.Json;

namespace ChatClient.Services.Client.Parsers
{
    internal class MessageParser
    {

        public Message Parse(string jsonMsg)
        {
             return JsonSerializer.Deserialize<Message>(jsonMsg);
        }
    }
}
