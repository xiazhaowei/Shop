using System;
using System.ServiceProcess;

namespace Shop.MessageBroker
{
    static class Program
    {
        static void Main()
        {
            if (!Environment.UserInteractive)//Windows服务
            {
                ServiceBase.Run(new Service1());
            }
            else//控制台启动
            {
                Bootstrap.Initialize();
                Bootstrap.Start();
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
