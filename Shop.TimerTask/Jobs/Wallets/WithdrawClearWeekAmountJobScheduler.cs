using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Shop.TimerTask.Jobs.Wallets
{
    /// <summary>
    /// 计划任务调度
    /// </summary>
    public class WithdrawClearWeekAmountJobScheduler
    {

        public static void Start()
        {
            Debug.WriteLine("付款过期订单任务启动");

            IJobDetail job = JobBuilder.Create<WithdrawClearWeekAmountJob>()
                                  .WithIdentity("WithdrawClearWeekAmountJob")
                                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithDailyTimeIntervalSchedule
                                              (s =>
                                                 s.WithIntervalInSeconds(30)
                                                .OnEveryDay()
                                              )
                                             .ForJob(job)
                                             .WithIdentity("WithdrawClearWeekAmountJobTrigger")
                                             .StartNow()
                                             .WithCronSchedule("0 15 23 ? * SUN")//每周日晚上23：15 执行
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);
            sc.Start();
        }
    }
}