using ChatClient.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;


namespace ChatClient.Services.Client.Parsers
{
    internal class UserCollectionParser
    {
        private string _message;
        public ICollection<User> Parse(Message message)
        {
            string msg = message.Text;
            ICollection<User> collection = new List<User>();
            _message = msg;
            MessageContainsNull();
            msg = msg[0..^1];
            List<string> list = msg.Split('/').ToList();
            for(int i = 0; i < list.Count; i++)
            {
                collection.Add(JsonSerializer.Deserialize<User>(list[i]));
            }
            return collection;
        }
        private void MessageContainsNull()
        {
            if(_message.Contains("null"))
            {
                throw new JsonException("Не удалось разобрать сообщение, не ожидалось содержание \"null\"");
            }
        }
    }
}
