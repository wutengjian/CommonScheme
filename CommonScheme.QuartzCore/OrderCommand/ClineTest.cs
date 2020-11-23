using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.QuartzCore.OrderCommand
{
    public class ClineTest
    {
        public static void Run()
        {
            Combination combination = new Combination();
            Invoker invoker = combination.GetInvoker();
            List<Command> commands = combination.CommandFactory();
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                invoker.SetCommand(commands[r.Next(commands.Count)]);
            }
            invoker.ExecuteCommand();
            invoker.RemoveCommand(commands[0]);
        }
    }
}
