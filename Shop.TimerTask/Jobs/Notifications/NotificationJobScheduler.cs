using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Shop.TimerTask.Jobs.Notifications
{
    /// <summary>
    /// 计划任务调度
    /// </summary>
    public class NotificationJobScheduler
    {

        public static void Start()
        {
            IJobDetail job = JobBuilder.Create<NotificationJob>()
                                  .WithIdentity("NotificationJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("NotificationJobTrigger")
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