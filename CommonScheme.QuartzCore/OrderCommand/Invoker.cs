using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.QuartzCore.OrderCommand
{
    public class Invoker
    {
        private List<Command> commands;
        public Invoker()
        {
            commands = new List<Command>();
        }
        /// <summary>
        /// 获取已创建的命令
        /// </summary>
        /// <returns></returns>
        public List<Command> GetCommands()
        {
            return commands;
        }
        public void SetCommand(Command command)
        {
            Console.WriteLine("正在添加单个命令");
            commands.Add(command);
        }
        public void SetCommand(List<Command> commands)
        {
            Console.WriteLine("正在添加多个命令");
            commands.AddRange(commands);
        }
        public void RemoveCommand( Command command)
        {
            Console.WriteLine("取消单个命令");
            // commands.Remove(command);
        }
        public void RemoveCommand( List<Command> commands)
        {
            Console.WriteLine("取消多个命令");
            foreach (var command in commands)
            {
                //this.commands.Remove(command);
            }
        }
        public void ExecuteCommand()
        {
            Console.WriteLine("提交并且执行命令");
            foreach (var command in commands)
            {
                command.Execute();
            }
        }
    }
}
