using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using ENode.SqlServer;
using Shop.Common;
using Shop.Domain.Models.Users;
using System;
using ECommonConfiguration = ECommon.Configurations.Configuration;
using ENode.Infrastructure;

namespace Shop.ProcessorHost
{
    /// <summary>
    /// 服务器容器
    /// </summary>
    public class Bootstrap
    {
        private static ILogger _logger;
        private static ECommonConfiguration _ecommonConfiguration;
        private static ENodeConfiguration _enodeConfiguration;

        /// <summary>
        /// 初始化服务
        /// </summary>
        public static void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeENode();
            }
            catch (Exception ex)
            {
                _logger.Error("ENode 初始化失败.", ex);
                throw;
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            try
            {
                _enodeConfiguration.StartEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("EQueue 启动失败.", ex);
                throw;
            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
            try
            {
                _enodeConfiguration.ShutdownEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("EQueue 停止失败.", ex);
                throw;
            }
        }

        /// <summary>
        /// 初始化 ECommon
        /// </summary>
        private static void InitializeECommon()
        {
            _ecommonConfiguration = ECommonConfiguration
                .Create()//创建一个ECommonConfiguration类的实例
                .UseAutofac()//使用的是Autofac容器
                .RegisterCommonComponents()//把一些默认的组件注入到容器中
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .BuildContainer();//告诉框架要捕获未处理的异常，这样框架在发现有未处理的异常时，会尝试记录错误日志

            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName);
            _logger.Info("ECommon 初始化成功.");
        }

        /// <summary>
        /// 初始化ENode
        /// </summary>
        private static void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Shop.Common"),
                Assembly.Load("Shop.Domain"),
                Assembly.Load("Shop.Commands"),
                Assembly.Load("Shop.CommandHandlers"),
                Assembly.Load("Shop.ProcessManagers"),
                Assembly.Load("Shop.ReadModel"),
                Assembly.Load("Shop.Repositories.Dapper"),
                Assembly.GetExecutingAssembly()
            };
            var setting = new ConfigurationSetting(ConfigSettings.ENodeConnectionString);
            _enodeConfiguration = _ecommonConfiguration
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerLockService()
                .UseSqlServerEventStore()
                .UseSqlServerPublishedVersionStore()
                .UseEQueue()
                .InitializeSqlServerEventStore()
                .InitializeSqlServerLockService()
                .InitializeSqlServerPublishedVersionStore()
                .InitializeBusinessAssemblies(assemblies)
                .BuildContainer();

            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(UserMobileIndex).Name);
            _logger.Info("ENode 初始化成功.");
        }
    }
}
