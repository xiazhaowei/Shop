using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using Shop.Common;
using Shop.TimerTask.Jobs.Notifications;
using Shop.TimerTask.Jobs.Orders;
using Shop.TimerTask.Jobs.Partners;
using Shop.TimerTask.Jobs.SearchEngine;
using Shop.TimerTask.Jobs.StoreOrders;
using Shop.TimerTask.Jobs.Wallets;
using System;
using System.Reflection;

namespace Shop.TimerTask
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
                ConfigSettings.Initialize();
                InitializeENode();
                //启动定时任务
                StartTimerTasks();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
                _enodeConfiguration.StartEQueue();
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
                _enodeConfiguration.ShutdownEQueue();
        }
        
        /// <summary>
        /// 初始化ENode
        /// </summary>
        private static void InitializeENode()
        {
            var assemblies = new[]
            {
                Assembly.Load("Shop.Commands"),
                Assembly.Load("Shop.QueryServices"),
                Assembly.Load("Shop.QueryServices.Dapper"),
                Assembly.GetExecutingAssembly()
            };

            _enodeConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .BuildContainer()
                .InitializeBusinessAssemblies(assemblies);

            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName).Info("ENode initialized.");
        }

        private static void StartTimerTasks()
        {
            OrderJobScheduler.Start();                                                          //预订单30分钟付款到期
            WithdrawClearWeekAmountJobScheduler.Start();                        //每周日晚上清空本周提现金额
            ResetTodayStatisticJobScheduler.Start();                                        //重置钱包统计信息
            Jobs.Stores.ResetTodayStatisticJobScheduler.Start();                        //重置线上店铺统计信息
            Jobs.OfflineStores.ResetTodayStatisticJobScheduler.Start();             //重置线下统计信息
            IncentiveJobScheduler.Start();                                                      //善心激励
            CreateSearchIndexJobScheduler.Start();                                      //创建索引
            BalanceJobScheduler.Start();                                                        //代理分红
            StoreOrderJobScheduler.Start();                                                    //10D自动确认收货
            NotificationJobScheduler.Start();                                                   //10M 发送通知短信
            //OrderGoodsJobScheduler.Start();                                               //商品服务自动过期服务
        }
        
    }
}
