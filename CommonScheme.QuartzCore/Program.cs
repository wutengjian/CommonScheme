using System;
using System.Threading;

namespace CommonScheme.QuartzCore
{
    class Program
    {
        static void Main(string[] args)
        {
            SchedulerMonitor monitor = new SchedulerMonitor();
            monitor.FactoryJob();
            monitor.StartJob();
            Thread.Sleep(1000 * 10);
            string name = "JobTestA";
            string group = "JobTest";
            monitor.PauseJob(name, group);
            monitor.UpdateJobTime(name, " 0/5 * * * * ? ", group);
            Thread.Sleep(1000 * 10);
            monitor.RemoveJob(name,group);
            while (true)
            {
                if (Console.ReadLine() == "exit") break;
            }
            Console.WriteLine("运行完成");
        }
    }
}
