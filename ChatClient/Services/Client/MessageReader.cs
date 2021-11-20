using ChatClient.Models;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ChatClient.Services.Client.Parsers;
using System.Collections.Generic;

namespace ChatClient.Services.Client
{
    internal class MessageReader
    {
        private NetworkStream _netStream;

        public MessageReader(NetworkStream netStream)
        {
            _netStream = netStream;
        }

        public Message Read()
        {
            Message msg = new Message();
            BinaryReader br = new BinaryReader(_netStream, Encoding.Default, true);
            using(br)
            {
                msg = Parse(br.ReadString());
            }
            return msg;
        }

        private Message Parse(string msg)
        {
            MessageParser parser = new MessageParser();
            return parser.Parse(msg);
        }
    }
}
