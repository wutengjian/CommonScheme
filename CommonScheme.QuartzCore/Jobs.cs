using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace CommonScheme.QuartzCore
{
    [DisallowConcurrentExecution]
    public class JobTestA : IJob
    {

        public JobTestA()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
            Thread.Sleep(3000);
            Console.WriteLine("TestJobA"+DateTime.Now.ToString("mm:ss"));
            return Task.Run(null);
        }
    }
}
