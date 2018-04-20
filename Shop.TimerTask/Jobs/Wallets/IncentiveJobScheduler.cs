using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.Wallets
{
    public class IncentiveJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<IncentiveJob>()
                                  .WithIdentity("IncentiveJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("IncentiveJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 00 23 ? * 2,3,4,5,1") //每日23日分红
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
