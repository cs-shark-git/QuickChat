using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
    internal class ConnectDialogViewModel : ViewModel
    {

        private string _adress;
        private string _nickName;
        public string Adress
        {
            get => _adress;
            set => Set(ref _adress, value);
        }

        public string NickName
        {
            get => _nickName;
            set => Set(ref _nickName, value);
        }

        public ConnectDialogViewModel()
        {
            _adress = "dss";
            _nickName = "ds3F5DDSSDFGfews";
        }
    }
}
