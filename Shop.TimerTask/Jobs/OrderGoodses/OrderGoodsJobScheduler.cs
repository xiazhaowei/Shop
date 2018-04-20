using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Shop.TimerTask.Jobs.OrderGoodses
{
    /// <summary>
    /// 计划任务调度
    /// </summary>
    public class OrderGoodsJobScheduler
    {

        public static void Start()
        {
            Debug.WriteLine("订单商品服务任务启动");

            IJobDetail job = JobBuilder.Create<ExpiredOrderGoodsJob>()
                                  .WithIdentity("ExpiredOrderGoodsJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("ExpiredOrderGoodsJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 0/5 * * * ?")//每一分钟执行一次
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}