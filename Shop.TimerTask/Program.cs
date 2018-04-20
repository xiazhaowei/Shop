using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Shop.TimerTask
{
    class Program
    {
        static void Main(string[] args)
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
