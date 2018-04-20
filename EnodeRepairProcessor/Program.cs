using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using EnodeRepairProcessor.QueryServices;
using EnodeRepairProcessor.Services;
using Shop.Common;
using System;
using System.Reflection;

namespace EnodeRepairProcessor
{
    class Program
    {
        static ILogger _logger;

        static void Main(string[] args)
        {
            InitializeECommonFramework();

            DoRepair();

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
        
        /// <summary>
        /// 初始化框架
        /// </summary>
        static void InitializeECommonFramework()
        {
            ConfigSettings.Initialize();
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly()
            };
            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .UseMyComponents()
                .BuildContainer()
                .RegisterUnhandledExceptionHandler();


            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program).Name);
            _logger.Info("Ecommon Initialized...");
        }

        static void DoRepair()
        {
            var aggregateRootQueryService= ObjectContainer.Resolve<IAggregateRootQueryService>();
            var generator = ObjectContainer.Resolve<IGenerator>();
            var repairProcessor = new RepairProcessor(generator, aggregateRootQueryService);
            
            _logger.Info("Doing Repair...");
            repairProcessor.DoWarkReferToNodeDb();
            _logger.Info("Repair Complete...");
        }
    }
}
