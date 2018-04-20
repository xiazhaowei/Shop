using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using ENode.Infrastructure;
using ENode.SqlServer;
using Shop.Domain.Models.Users;
using Shop.Common;

namespace Shop.CommandService
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeENode();
            InitializeCommandService();
        }
        public static void Start()
        {
            _enodeConfiguration.StartEQueue();
        }
        public static void Stop()
        {
            _enodeConfiguration.ShutdownEQueue();
        }

        private static void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Shop.Common"),
                Assembly.Load("Shop.Domain"),
                Assembly.Load("Shop.Commands"),
                Assembly.Load("Shop.CommandHandlers"),
                Assembly.Load("Shop.Repositories.Dapper"),
                Assembly.GetExecutingAssembly()
            };
            var setting = new ConfigurationSetting(ConfigSettings.ENodeConnectionString);
            

            _enodeConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerEventStore()
                .UseSqlServerLockService()
                .UseEQueue()
                .BuildContainer()
                .InitializeSqlServerEventStore()
                .InitializeSqlServerLockService()
                .InitializeBusinessAssemblies(assemblies);
        }
        private static void InitializeCommandService()
        {
            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(UserMobileIndex).Name);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program)).Info("命令处理服务 initialized.");
        }
    }
}
