using Quartz;
using Quartz.Impl;

namespace Shop.TimerTask.Jobs.SearchEngine
{
    public class CreateSearchIndexJobScheduler
    {
        public static void Start()
        {

            IJobDetail job = JobBuilder.Create<CreateSearchIndexJob>()
                                  .WithIdentity("CreateSearchIndexJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("CreateSearchIndexJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 0/30 * * * ?")//每30分钟创建索引
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}
