using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Shop.TimerTask.Jobs.Orders
{
    /// <summary>
    /// 计划任务调度
    /// </summary>
    public class OrderJobScheduler
    {

        public static void Start()
        {
            IJobDetail job = JobBuilder.Create<OrderJob>()
                                  .WithIdentity("OrderJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("OrderJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 0/5 * * * ?")//每5分钟执行一次
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}