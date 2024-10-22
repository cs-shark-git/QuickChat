﻿namespace ChatClient.Models
{
    internal enum MessageType
    {
        Default,
        UserConnection,
        UserDisconnection
    }

    internal class Message
    {

        public string Text
        {
            get => _text;
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
            }
        }
        private string _text;

        public MessageType Type { get; set; }
    }
}