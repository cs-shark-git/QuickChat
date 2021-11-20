using ChatClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace ChatClient.Services.Client.Parsers
{
    internal class UserCollectionParser
    {
        private string _message;

        public ICollection<User> Parse(string message)
        {
            ICollection<User> collection = new List<User>();
            _message = message;
            MessageContainsNull();
            message = message[0..^1];
            List<string> list = message.Split('/').ToList();
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
