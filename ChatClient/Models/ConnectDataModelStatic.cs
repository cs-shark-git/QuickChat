using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Models
{
    static class ConnectDataModelStatic
    {
        static public int Port { get =>  _port; set => _port = value; }
        static private int _port;
        static public string Name { get => _name; set => _name = value; }
        static private string _name;
        static public string Adress
        {
            get => _adress;
            set
            {
                _adress = value;
                if (Adress.Contains(':'))
                {
                    if (Adress.LastIndexOf(':') + 1 < Adress.Length)
                    {
                        bool error;
                        error = int.TryParse(Adress.Substring(Adress.LastIndexOf(':') + 1), out _port);
                        if (!error)
                            MessageBox.Show("Please input integer numbers after ':' !", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                        Adress = Adress.Substring(0, Adress.LastIndexOf(':'));
                    }
                }
            }
        }
        static private string _adress;
    }
}
