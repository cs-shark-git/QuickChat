using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Services.Client.Parsers;

namespace ChatClient.Services.Client
{
    class MessageReader
    {
        private NetworkStream _netStream;

        public MessageReader(NetworkStream netStream)
        {
            _netStream = netStream;
        }

        public Message ReadMessage()
        {
            Message msg = new Message();
            BinaryReader br = new BinaryReader(_netStream, Encoding.Default, true);
            using(br)
            {
                msg.Text = br.ReadString();
            }
            return msg;
        }

        public Message ReadMessages(ref ICollection<Message> col)
        {
            Message msg = new Message();
            BinaryReader br = new BinaryReader(_netStream, Encoding.Default, true);
            using(br)
            {
                while(true)
                {
                    msg.Text = br.ReadString();
                    col.Add(msg);
                }
            }
        }
    }
}
