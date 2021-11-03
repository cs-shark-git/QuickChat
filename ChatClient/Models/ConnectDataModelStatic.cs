using ChatClient.Services;
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
        public static int Port => _port;
        private static int _port;

        public static string Name => _name;
        private static string _name;

        public static string Adress => _adress;
        private static string _adress;

        public static bool SetValues(string adress, string name)
        {
            ConnectionDataValidator connectionDataValidator = new ConnectionDataValidator();
            if(connectionDataValidator.Validate(adress, name))
            {
                _adress = connectionDataValidator.Adress;
                _name = name;
                _port = connectionDataValidator.Port;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
