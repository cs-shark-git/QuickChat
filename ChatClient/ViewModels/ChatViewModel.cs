using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
    class ChatViewModel : ViewModel
    {
        private string _title = "QuickChat: v 0.0.1";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public ChatViewModel()
        {

        }
    }
}
