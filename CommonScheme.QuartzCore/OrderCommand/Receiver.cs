using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.QuartzCore.OrderCommand
{
    public abstract class Receiver
    {
        public abstract void ActionTodo();
    }
    public class ReceiverA : Receiver
    {
        public override void ActionTodo()
        {
            Console.WriteLine("ReceiverA正在执行命令");
        }
    }
}
