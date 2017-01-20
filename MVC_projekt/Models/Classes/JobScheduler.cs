using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace MVC_projekt.Models.Classes
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ReturnRemainderEmail>().Build();
            IJobDetail jobB = JobBuilder.Create<ReturnDelayEmail>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 0))
                  )
                .Build();

            ITrigger triggerB = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s =>
                        s.WithIntervalInHours(24)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 0))
                )
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.ScheduleJob(jobB, triggerB);
        }
    }
}