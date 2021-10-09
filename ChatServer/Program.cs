using System;

namespace ChatServer
{
    class Program
    {
        static void Main()
        {
            int port = 49276;
            Server server = new Server(port);
            Console.WriteLine("Server started...");
            try
            {
                server.Listen();
            }
            catch (Exception ex)
            {
                server.Shutdown();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
