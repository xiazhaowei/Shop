using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.Stores
{
    public class ResetTodayStatisticJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<ResetTodayStatisticJob>()
                                  .WithIdentity("StoreResetTodayStatisticJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("StoreResetTodayStatisticJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 5 0 * * ?")//每日零时5分执行
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
