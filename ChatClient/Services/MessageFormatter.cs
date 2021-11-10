using System.Text;

namespace ChatClient.Services
{
    internal static class MessageFormatter
    {
        public static string SplitMessageOnLines(string msg, int n, string name)
        {
            StringBuilder sb = new StringBuilder(msg.Length + (msg.Length + 9) / 10);
            string space = AddSpaces(name);

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
            {
                --sb.Length;
            }

            return sb.ToString();
        }

        private static string AddSpaces(string name)
        {
            
            string space = " ";
            for(int i = 0; i < name.Length; i++)
            {
                space += " ";
            }
            space += space;
            return space;
        }
    }
}
