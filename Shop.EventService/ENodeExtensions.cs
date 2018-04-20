using ECommon.Socketing;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Infrastructure;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using Shop.Common;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Shop.EventService
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static DomainEventConsumer _eventConsumer;
        private static PublishableExceptionPublisher _exceptionPublisher;
        private static PublishableExceptionConsumer _exceptionConsumer;

        public static ENodeConfiguration BuildContainer(this ENodeConfiguration enodeConfiguration)
        {
            enodeConfiguration.GetCommonConfiguration().BuildContainer();
            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };
            enodeConfiguration.RegisterTopicProviders(assemblies);

            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.RegisterEQueueComponents();

            _commandService = new CommandService();
            _exceptionPublisher = new PublishableExceptionPublisher();

            _eventConsumer = new DomainEventConsumer();
            _exceptionConsumer = new PublishableExceptionConsumer();

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IMessagePublisher<IPublishableException>, PublishableExceptionPublisher>(_exceptionPublisher);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var nameServerEndpoint = new IPEndPoint(SocketUtils.GetLocalIPV4(), ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };
            //命令生产者
            _commandService.Initialize(setting: new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });
            //异常生产者
            _exceptionPublisher.Initialize(setting:new ProducerSetting {
                NameServerList=nameServerEndpoints
            });

            //事件消费者
            _eventConsumer.Initialize(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _eventConsumer.Subscribe(Topics.ShopDomainEventTopic);

            //异常消费者
            _exceptionConsumer.Initialize(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _exceptionConsumer.Subscribe(Topics.ShopExceptionTopic);

            _commandService.Start();
            _eventConsumer.Start();
            _exceptionPublisher.Start();
            _exceptionConsumer.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _commandService.Shutdown();
            _exceptionPublisher.Shutdown();
            _exceptionConsumer.Shutdown();

            return enodeConfiguration;
        }
    }
}
