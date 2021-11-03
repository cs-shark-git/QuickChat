using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Services
{
    internal class ConnectionDataValidator
    {
        public int Port => _port;
        public string Adress => _adress;

        private bool _validationStatus;

        private string _adress;
        private string _name;
        private int _port;

        public bool Validate(string adress, string name)
        {
            _validationStatus = true;

            NullOrEmptyCheck(name, ref _name);
            NullOrEmptyCheck(adress, ref _adress);
            AdressValidationCheck();
            return _validationStatus;
        }
        private void NullOrEmptyCheck(string value, ref string field)
        {
            if(string.IsNullOrEmpty(value))
            {
                MessageBox.Show("This field can't be empty", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                _validationStatus = false;
            }
            field = value;
        }

        private void AdressValidationCheck()
        {
            if(_adress.Contains(':'))
            {
                if(_adress.LastIndexOf(':') + 1 < _adress.Length)
                {
                    TrySetPort();
                }
                else
                {
                    MessageBox.Show("Please input integer numbers (port) after ':'", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            else
            {
                MessageBox.Show("Adress must contains ':'", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void TrySetPort()
        {
            bool error;
            error = int.TryParse(_adress[(_adress.LastIndexOf(':') + 1)..], out _port);

            if(!error)
            {
                MessageBox.Show("Please input port value (integer numbers)", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                _validationStatus = false;
            }
            else
            {
                CanConnectToAdressCheck();
            }
        }

        private void CanConnectToAdressCheck()
        {
            _adress = _adress.Substring(0, _adress.LastIndexOf(':'));
            if(!TryConnect(_adress))
            {
                MessageBox.Show("Impossible connect to this adress", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                _adress = "";
                _validationStatus = false;
            }
        }

        private bool TryConnect(string ip)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(ip, _port);
            }
            catch(Exception)
            {
                return false;
            }
            tcpClient.Close();
            return true;
        }
    }
}
