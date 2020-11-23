using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace CommonScheme.QuartzCore
{
    public class SchedulerMonitor
    {
        IScheduler _scheduler;
        public SchedulerMonitor()
        {
            ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();
            _scheduler = _schedulerFactory.GetScheduler().Result;
        }
        public void Start()
        {
            ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();//1、声明一个调度工厂
            _scheduler = _schedulerFactory.GetScheduler().Result;//2、通过调度工厂获得调度器
            _scheduler.Start();//3、开启调度器
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever()).Build();//4、创建一个触发器，2秒执行一次
            var jobDetail = JobBuilder.Create<JobTestA>().WithIdentity("job", "group").Build();//5、创建任务                                                                                              
            _scheduler.ScheduleJob(jobDetail, trigger); //6、将触发器和任务器绑定到调度器中
        }
        public void StartJob()
        {
            _scheduler.Start();
        }
        public void StopJob()
        {
            _scheduler.Shutdown();
        }

        public async void FactoryJob()
        {
            //0 0 12 * * ? 每天中午12点触发
            //0 15 10 ? **每天上午10 : 15触发
            //0 15 10 * * ? 每天上午10 : 15触发
            //0 15 10 * * ? *每天上午10 : 15触发
            //0 15 10 * * ? 2005                          2005年的每天上午10: 15触发
            //0 * 14 * * ? 在每天下午2点到下午2 : 59期间的每1分钟触发
            //0 0 / 5 14 * * ? 在每天下午2点到下午2 : 55期间的每5分钟触发
            //0 0 / 5 14,18 * * ? 在每天下午2点到2 : 55期间和下午6点到6: 55期间的每5分钟触发
            //0 0 - 5 14 * * ? 在每天下午2点到下午2 : 05期间的每1分钟触发
            //0 10,44 14 ? 3 WED                          每年三月的星期三的下午2: 10和2: 44触发
            //0 15 10 ? *MON - FRI                         周一至周五的上午10:15触发
            //0 15 10 15 * ? 每月15日上午10 : 15触发
            //0 15 10 L * ? 每月最后一日的上午10 : 15触发
            //0 15 10 L - 2 * ? 每个月的第二天到最后一天的上午10 : 15触发
            //0 15 10 ? *6L                              每月的最后一个星期五上午10:15触发
            //0 15 10 ? *6L                              每个月最后一个星期五上午10时15分触发
            //0 15 10 ? *6L 2002 - 2005                    2002年至2005年的每月的最后一个星期五上午10: 15触发
            //0 15 10 ? *6#3								每月的第三个星期五上午10:15触发
            //0 0 12 1 / 5 * ? 每月每隔5天下午12点（中午）触发, 从每月的第一天开始
            //0 11 11 11 11 ? 每11月11日上午11时11分触发

            await CreateJob<JobTestA>("JobTestA", "JobTest", " 0/1 * * * * ? ");
        }
        public async Task CreateJob<T>(string name, string group, string cronTime) where T : IJob
        {
            var job = JobBuilder.Create<T>().WithIdentity(name, group).Build();
            var tigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(name, group).StartNow()
                .WithCronSchedule(cronTime).Build();
            await _scheduler.ScheduleJob(job, tigger);//把作业和触发器放入调度器中
        }

        public async void UpdateJobTime(string name, string cronTime, string group = null)
        {
         
            group = string.IsNullOrEmpty(group) ? name : group;
            TriggerKey TKey = new TriggerKey(name, group);
            var tigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(name, group)
                .WithCronSchedule(cronTime).Build();
            await _scheduler.RescheduleJob(TKey, tigger);
        }
        public async void PauseJob(string name, string group = null)
        {
            group = string.IsNullOrEmpty(group) ? name : group;
            JobKey JKey = new JobKey(name, group);
            await _scheduler.PauseJob(JKey);
        }
        public async void RemoveJob(string name, string group = null)
        {
            group = string.IsNullOrEmpty(group) ? name : group;
            JobKey JKey = new JobKey(name, group);
            TriggerKey TKey = new TriggerKey(name, group);
            await _scheduler.PauseTrigger(TKey);// 停止触发器
            await _scheduler.UnscheduleJob(TKey);// 移除触发器
            await _scheduler.DeleteJob(JKey);// 删除任务 
        }
    }
}
