using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Models
{
    static class ConnectDataModelStatic
    {
        static public int Port { get => _port; set => _port = value; }
        static private int _port;
        static public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (string.IsNullOrEmpty(_name))
                {
                    MessageBox.Show("Name can't be empty", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                    _name = null;
                    ValidationStatus = false;
                    return;
                }
            }
        }
        static private string _name;
        static public string Adress
        {
            get => _adress;
            set
            {
                _adress = value;
                if (string.IsNullOrEmpty(_adress))
                {
                    MessageBox.Show("Adress can't be empty!", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                    _adress = null;
                    ValidationStatus = false;
                    return;
                }

                if (Adress.Contains(':'))
                {
                    if (Adress.LastIndexOf(':') + 1 < Adress.Length)
                    {
                        bool error;
                        error = int.TryParse(Adress.Substring(Adress.LastIndexOf(':') + 1), out _port);

                        if (!error)
                        {
                            MessageBox.Show("Please input integer numbers after ':' !", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                            ValidationStatus = false;
                        }
                        else
                        {
                            Adress = Adress.Substring(0, Adress.LastIndexOf(':'));
                            if (!TryConnect(Adress))
                            {
                                MessageBox.Show("Impossible connect to this adress", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                                _adress = null;
                                ValidationStatus = false;
                            }
                        }

                    }
                }
            }
        }
        static private string _adress;

        static public bool ValidationStatus { get; set; }

        static private bool TryConnect(string ip)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(ip, Port);
            }
            catch (Exception)
            {
                return false;
            }
            tcpClient.Close();
            return true;
        }
        static ConnectDataModelStatic()
        {
            ValidationStatus = true;
        }
    }
}
