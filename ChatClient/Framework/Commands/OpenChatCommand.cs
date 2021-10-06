using ChatClient.Framework.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Framework.Commands
{
    class OpenChatCommand : Command
    {
        private Chat _window;

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            _window = new Chat()
            {
                Owner = Application.Current.MainWindow
            };
            _window.ShowDialog();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _window = null;
        }
    }
}
