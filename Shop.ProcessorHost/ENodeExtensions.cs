using System.Net;
using System.Collections.Generic;
using ECommon.Socketing;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using Shop.Common;
using ENode.Commanding;

namespace Shop.ProcessorHost
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static ApplicationMessagePublisher _applicationMessagePublisher;
        private static DomainEventPublisher _domainEventPublisher;
        private static PublishableExceptionPublisher _exceptionPublisher;

        private static CommandConsumer _commandConsumer;
        private static DomainEventConsumer _eventConsumer;
        private static ApplicationMessageConsumer _applicationMessageConsumer;
        private static PublishableExceptionConsumer _exceptionConsumer;

        public static ENodeConfiguration BuildContainer(this ENodeConfiguration enodeConfiguration)
        {
            enodeConfiguration.GetCommonConfiguration().BuildContainer();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            //生产者设置
            var producerSetting = new ProducerSetting
            {
                NameServerList = new List<IPEndPoint> { new IPEndPoint(SocketUtils.GetLocalIPV4(), ConfigSettings.NameServerPort) }
            };
            //消费者设置
            var consumerSetting = new ConsumerSetting
            {
                NameServerList = new List<IPEndPoint> { new IPEndPoint(SocketUtils.GetLocalIPV4(), ConfigSettings.NameServerPort) }
            };

            //命令生成者
            _commandService = new CommandService();
            _commandService.Initialize(setting: producerSetting);
            //事件生产者
            _domainEventPublisher = new DomainEventPublisher().Initialize(setting: producerSetting);
            //消息生产者
            _applicationMessagePublisher = new ApplicationMessagePublisher().Initialize(setting: producerSetting);
            //异常生产者
            _exceptionPublisher = new PublishableExceptionPublisher().Initialize(setting: producerSetting);

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_domainEventPublisher);
            configuration.SetDefault<IMessagePublisher<IApplicationMessage>, ApplicationMessagePublisher>(_applicationMessagePublisher);
            configuration.SetDefault<IMessagePublisher<IPublishableException>, PublishableExceptionPublisher>(_exceptionPublisher);
            
            //命令消费者
            _commandConsumer = new CommandConsumer().Initialize(setting: consumerSetting)
                .Subscribe(Topics.ShopCommandTopic);
            //事件消费者
            _eventConsumer = new DomainEventConsumer().Initialize(setting: consumerSetting)
                .Subscribe(Topics.ShopDomainEventTopic);

            //消息消费者
            _applicationMessageConsumer = new ApplicationMessageConsumer().Initialize(setting: consumerSetting)
                .Subscribe(Topics.ShopApplicationMessageTopic);
            //异常消费者
            _exceptionConsumer = new PublishableExceptionConsumer().Initialize(setting: consumerSetting)
                .Subscribe(Topics.ShopExceptionTopic);

            return enodeConfiguration;
        }


        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {


            _commandService.Start();
            _domainEventPublisher.Start();
            _applicationMessagePublisher.Start();
            _exceptionPublisher.Start();

            _commandConsumer.Start();
            _eventConsumer.Start();
            _applicationMessageConsumer.Start();
            _exceptionConsumer.Start();

            return enodeConfiguration;
        }

        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Shutdown();
            _applicationMessagePublisher.Shutdown();
            _domainEventPublisher.Shutdown();
            _exceptionPublisher.Shutdown();

            _commandConsumer.Shutdown();
            _eventConsumer.Shutdown();
            _exceptionConsumer.Shutdown();
            _applicationMessageConsumer.Shutdown();

            return enodeConfiguration;
        }
    }
}
