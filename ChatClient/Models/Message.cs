using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Models
{
    class Message
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
