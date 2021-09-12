using ChatClient.Models;
using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        public ChatViewModel()
        {
            Users = new List<User>()
            {
                new User()
                {
                    NickName = "User256"
                },
                new User()
                {
                    NickName = "CS-Shark"
                },
                new User()
                {
                    NickName = "Anonim"
                }
            };
        }
    }
}
