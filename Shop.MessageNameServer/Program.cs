using System;
using System.ServiceProcess;

namespace Shop.MessageNameServer
{
    static class Program
    {
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(new Service1());
            }
            else
            {
                Bootstrap.Initialize();
                Bootstrap.Start();
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
