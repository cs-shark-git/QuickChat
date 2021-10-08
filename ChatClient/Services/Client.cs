using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ChatClient.Models;

namespace ChatClient.Service
{
    class Client
    {
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        private string _name;

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            private set
            {
                _messages = value;
            }
        }
        private ObservableCollection<Message> _messages;

        public delegate void MessageListChanged(ObservableCollection<Message> messageList);
        public event MessageListChanged OnMessageListChanged;

        private string _host;
        private int _port;

        private TcpClient _tcpClient;
        private NetworkStream _netStream;
        private Dispatcher _dispatcher;


        public Client(string host, int port, Dispatcher dispatcher)
        {
            Messages = new ObservableCollection<Message>();
            _tcpClient = new TcpClient();

            _host = host;
            _port = port;

            _dispatcher = dispatcher;
        }

        public void Start()
        {
            _tcpClient.Connect(_host, _port);
            _netStream = _tcpClient.GetStream();
            Procces();
        }

        private void Procces()
        {
            SendMessage(_name);
            Task task = new Task(ReceiveMessages);
            task.Start();
        }

        public void SendMessage(string message)
        {
            BinaryWriter bw = new BinaryWriter(_netStream, Encoding.Default, true);
            using (bw)
            {
                bw.Write(message);
            }
        }

        private void ReceiveMessages()
        {
            var br = new BinaryReader(_netStream, Encoding.Default, true);

            while (true)
            {
                try
                {
                    string text = br.ReadString();

                    Message message = new Message();
                    message.Text = text;
                    _dispatcher.Invoke(new Action(() =>
                    {
                        _messages.Add(message);
                    }));
                    OnMessageListChanged(Messages);
                }
                catch (IOException)
                {
                    break;
                }
            }
        }

        public void Stop()
        {
            if (_netStream != null)
                _netStream.Close();
            if (_tcpClient != null)
                _tcpClient.Close();        
        }
    }
}
