using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using ENode.SqlServer;
using Shop.Common;

namespace Shop.EventService
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeENode();
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
                Assembly.Load("Shop.ProcessManagers"),
                Assembly.Load("Shop.Denormalizers.Dapper"),
                Assembly.GetExecutingAssembly()
            };
            var setting = new ConfigurationSetting(ConfigSettings.ENodeConnectionString);

            _enodeConfiguration = Configuration
                .Create()//初始化ecommon
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode(setting)//初始化enode
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerPublishedVersionStore()
                .UseEQueue()
                .BuildContainer()
                .InitializeSqlServerPublishedVersionStore()
                .InitializeBusinessAssemblies(assemblies);

            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program)).Info("事件处理服务 initialized.");
        }
    }
}
