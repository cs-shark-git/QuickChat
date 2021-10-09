using ChatClient.Models;
using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Service;
using System.Windows.Input;
using ChatClient.Framework.Commands;
using ChatClient.Framework;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace ChatClient.ViewModels
{
    internal class ChatViewModel : ViewModel
    {
        private string _title = "QuickChat: v 0.0.1";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string MessageText
        {
            get => _messageText;
            set => Set(ref _messageText, value);
        }
        private string _messageText;


        private Dispatcher _dispatcher;

        private string _name;
        private int _port;
        private string _adress;
        private Client _client;

        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        public ICommand DisconnectWindowCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    _client.Stop();
                    Application.Current.MainWindow.Show();
                }, (p) => true);
            }
        }
        public ICommand DisconnectCommand { get; }
        public ICommand SendMessageCommand { get; }

        private CloseWindowCommand _closeWindow;

        public bool DisconnectCommandCanExecute(object parameter) => _closeWindow.CanExecute(parameter);
        public void DisconnectCommandExecute(object parameter)
        {
            _client.Stop();
            Application.Current.MainWindow.Show();
            _closeWindow.Execute(parameter);
            
        }

        public void SendMessageCommandExecute(object parameter)
        {
            _client.SendMessage(MessageText);
            var msg = new Message();
            msg.Text = MessageText;
            Messages.Add(msg);
            MessageText = "";
        }

        public bool SendMessageCommandCanExecute(object parameter) => true;

        public ChatViewModel()
        {
            #region ...
            Users = new List<User>()
            {
                new User()
                {
                    Name = "User256"
                },
                new User()
                {
                    Name = "CS-Shark"
                },
                new User()
                {
                    Name = "Anonim"
                }
            };
            #endregion

            _closeWindow = new CloseWindowCommand();

            _dispatcher = Dispatcher.CurrentDispatcher;

            _port = ConnectDataModelStatic.Port;
            _adress = ConnectDataModelStatic.Adress;
            _name = ConnectDataModelStatic.Name;

            Messages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    Text = $"Подключение к чату завершено."
                },
                new Message()
                {
                    Text = $"{_name} подключился!"
                },
                new Message()
                {
                    Text = $"Добро пожаловать в чат, {_name}"
                }
            };

            SendMessageCommand = new RelayCommand(SendMessageCommandExecute, SendMessageCommandCanExecute);
            DisconnectCommand = new RelayCommand(DisconnectCommandExecute, DisconnectCommandCanExecute);
            _client = new Client(_adress, _port, _dispatcher);
            _client.Name = _name;
            _client.OnMessageListChanged += _client_OnMessageListChanged;
            _client.Start();
        }

        private void _client_OnMessageListChanged(ObservableCollection<Message> messageList)
        {
            Messages = messageList;
        }
    }
}
