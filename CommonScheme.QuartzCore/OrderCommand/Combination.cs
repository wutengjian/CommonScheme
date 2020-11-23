using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.QuartzCore.OrderCommand
{
    public class Combination
    {
        private Invoker invoker;
        public Combination()
        {
            invoker = new Invoker();
        }
        public Invoker GetInvoker()
        {
            return invoker;
        }
        public List<Command> CommandFactory()
        {
            List<Command> data = new List<Command>();
            Receiver receiver = new ReceiverA();
            data.Add(new ConcreteCommand(receiver));
            return data;
        }
    }
}
