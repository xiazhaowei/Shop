using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Shop.TimerTask.Jobs.StoreOrders
{
    /// <summary>
    /// 计划任务调度
    /// </summary>
    public class StoreOrderJobScheduler
    {

        public static void Start()
        {
            IJobDetail job = JobBuilder.Create<StoreOrderJob>()
                                  .WithIdentity("StoreOrderJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("StoreOrderJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 10 23 * * ?")//每日23：10分自动确认收货
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}