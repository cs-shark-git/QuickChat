using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    static class MessageFormatter
    {
        static public string AddSpaces(string msg, int n, string name)
        {
            var sb = new StringBuilder(msg.Length + (msg.Length + 9) / 10);
            string space = " ";
            for(int i = 0; i < name.Length; i++)
                space += " ";
            space += space;

            for(int q = 0; q < msg.Length;)
            {
                sb.Append(msg[q]);

                if(++q % n == 0)
                {
                    sb.AppendLine();
                    sb.Append(space);
                }
            }
            if(msg.Length % n == 0)
                --sb.Length;

            return sb.ToString();
        }
    }
}
