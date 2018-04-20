using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.OfflineStores
{
    public class ResetTodayStatisticJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<ResetTodayStatisticJob>()
                                  .WithIdentity("OfflineStoreResetTodayStatisticJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("OfflineStoreResetTodayStatisticJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 10 0 * * ?")//每日零时10分执行
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
