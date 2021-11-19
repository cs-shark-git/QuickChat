namespace ChatClient.Models
{
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
    }
}
