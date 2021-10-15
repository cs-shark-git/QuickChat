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
            private set
            {
                _messages = value;
            }
        }

        private const string CONNECT_MSG = "UC";
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
            SetUsers();
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
                        message.Text = $"[{DateTime.Now.Hour.ToString()}:{DateTime.Now.Minute.ToString()}:{DateTime.Now.Second.ToString()}] {user.Name} подключился к чату"; ;
                        _dispatcher.Invoke(new Action(() =>
                        {
                            _messages.Add(message);
                        }));
                        MessageListChanged(Messages);
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
                catch (IOException ex)
                {

                    Stop();
                    MessageBox.Show(ex.Message);
                    return;
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
    }
}
