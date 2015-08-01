using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace GitPluginVsix.Commands.Support
{
    public static class Commands
    {
        public static Dictionary<Type, ICommand> RegisterredCommands => new Dictionary<Type, ICommand>();

        public static void RegisterMenu<TCommand>(Guid commandSet, int commandId, Package package)
            where TCommand : ICommand, new()
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var command = RegisterCommand<TCommand>(package);
            
            var commandService = ((IServiceProvider)package).GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandId = new CommandID(commandSet, commandId);
                var menuItem = new MenuCommand(command.OnCommand, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        public static ICommand RegisterCommand<TCommand>(Package package)
            where TCommand : new()
        {
            ICommand command;
            if (!RegisterredCommands.TryGetValue(typeof(TCommand), out command))
            {
                RegisterredCommands[typeof(TCommand)] = command = (ICommand)new TCommand();
                command.Package = package;
            }

            return command;
        }
    }
}