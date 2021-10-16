using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            set
            {
                _messages = value;
            }
        }

        private const string CONNECT_MSG = "UC";
        private const string DISCONNECT_MSG = "UD";
        private ObservableCollection<Message> _messages;
        private ObservableCollection<User> _users;

        public delegate void MessageListChange(ObservableCollection<Message> messageList);
        public event MessageListChange MessageListChanged;

        public delegate void UserListChange(ObservableCollection<User> userList);
        public event UserListChange UserListChanged;

        private string _host;
        private int _port;

        private TcpClient _tcpClient;
        private NetworkStream _netStream;
        private Dispatcher _dispatcher;


        public Client(string host, int port, Dispatcher dispatcher)
        {
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    Text = $"Подключение к чату завершено."
                },
                new Message()
                {
                    Text = $"Добро пожаловать в чат, {_name}"
                }
            };
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
            SetUsers();
            Task task = new Task(ReceiveMessages);
            task.Start();
        }

        public void SendMessage(string message)
        {
            BinaryWriter bw = new BinaryWriter(_netStream, Encoding.Default, true);
            using(bw)
            {
                bw.Write(message);
            }
        }

        private void ReceiveMessages()
        {
            var br = new BinaryReader(_netStream, Encoding.Default, true);

            while(true)
            {
                try
                {
                    string msg = br.ReadString();

                    if(msg.Substring(0, 2) == CONNECT_MSG)
                    {
                        User user = new User();
                        user.Name = msg.Substring(2);
                        _dispatcher.Invoke(new Action(() =>
                        {
                            _users.Add(user);
                        }));
                        UserListChanged(_users);

                        Message message = new Message();
                        message.Text = $"{user.Name} подключился к чату";
                        _dispatcher.Invoke(new Action(() =>
                        {
                            _messages.Add(message);
                        }));
                        MessageListChanged(Messages);
                    }
                    else if(msg.Substring(0, 2) == DISCONNECT_MSG)
                    {
                        var user = FindUser(msg.Substring(2));

                        Message message = new Message();
                        message.Text = $"{user.Name} покинул чат";
                        _dispatcher.Invoke(new Action(() =>
                        {
                            _messages.Add(message);
                        }));
                        MessageListChanged(Messages);

                        _dispatcher.Invoke(new Action(() =>
                        {
                            _users.Remove(user);
                        }));
                        UserListChanged(_users);
                    }
                    else
                    {
                        Message message = new Message();
                        message.Text = msg;
                        _dispatcher.Invoke(new Action(() =>
                        {
                            _messages.Add(message);
                        }));
                        MessageListChanged(Messages);
                    }
                }
                catch(IOException)
                {

                    Stop();
                    return;
                }
            }
        }

        public void Stop()
        {
            if(_netStream != null)
                _netStream.Close();
            if(_tcpClient != null)
                _tcpClient.Close();
        }

        private int SetUsers()
        {
            string json;
            List<string> list = new List<string>();
            try
            {
                BinaryReader br = new BinaryReader(_netStream, Encoding.Default, true);
                json = br.ReadString();
                if(json.Contains("null"))
                    return -1;
                json = json.Substring(0, json.Length - 1);
                list = json.Split('/').ToList();
                for(int i = 0; i < list.Count; i++)
                {
                    _users.Add(JsonSerializer.Deserialize<User>(list[i]));
                }
                UserListChanged(_users);
                return 0;
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message);
                Stop();
                return -1;
            }
        }

        private User FindUser(string name)
        {
            foreach(var user in _users)
            {
                if(user.Name == name)
                    return user;
            }
            return null;
        }

        public string FormatMessage(string msg, int n)
        {
            var sb = new StringBuilder(msg.Length + (msg.Length + 9) / 10);
            string space = " ";
            for(int i = 0; i < _name.Length; i++)
                space += " ";
            space += space;

            for(int q = 0; q < msg.Length;)
            {
                sb.Append(msg[q]);

                if(++q % n == 0)
                {
                    sb.AppendLine();
                    sb.Append(space);
                }
            }
            if(msg.Length % n == 0)
                --sb.Length;

            return sb.ToString();
        }
    }
}
