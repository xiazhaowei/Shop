using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.Wallets
{
    public class ResetTodayStatisticJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<ResetTodayStatisticJob>()
                                  .WithIdentity("ResetTodayStatisticJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("ResetTodayStatisticJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 50 22 * * ?")//每日晚上22：50点执行
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
