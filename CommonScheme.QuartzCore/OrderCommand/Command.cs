using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.QuartzCore.OrderCommand
{
    public abstract class Command
    {
        protected Receiver _receiver;
        public Command(Receiver receiver)
        {
            _receiver = receiver;
        }
        public abstract void Execute();
    }
    public class ConcreteCommand : Command
    {
        public ConcreteCommand(Receiver receiver) : base(receiver) { }
        public override void Execute()
        {
            base._receiver.ActionTodo();
        }
    }
}
