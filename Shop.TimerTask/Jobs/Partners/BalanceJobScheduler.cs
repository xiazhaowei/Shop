using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.Partners
{
    public class BalanceJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<BalanceJob>()
                                  .WithIdentity("BalanceJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("BalanceJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 30 23 * * ?")//每日的23：30 执行分红程序
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
