using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace GitPluginVsix.Commands.Support
{
    public static class Commands
    {
        private static readonly Dictionary<CommandIdentifier, ICommand> RegisterredCommands = new Dictionary<CommandIdentifier, ICommand>();

        public static void RegisterCommand(ICommand command, Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var identifier = new CommandIdentifier(command.CommandSet, command.CommandId);

            var actualCommand = GetCommand(identifier);
            if (actualCommand == null)
            {
                RegisterredCommands[identifier] = actualCommand = command;
                actualCommand.Package = package;
            }

            var commandService = ((IServiceProvider)package).GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandId = new CommandID(identifier.CommandSet, identifier.CommandId);
                var menuItem = new OleMenuCommand(actualCommand.OnCommand, menuCommandId);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        private static void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            var menuitem = sender as OleMenuCommand;
            if (menuitem != null)
            {
                var command = GetCommand(menuitem.CommandID);
                if (command != null)
                {
                    menuitem.Enabled = command.IsEnabled();
                }
            }
        }

        private static ICommand GetCommand(CommandID commandSet)
        {
            return GetCommand(new CommandIdentifier(commandSet.Guid, commandSet.ID));
        }

        public static ICommand GetCommand(Guid commandSet, int commandId)
        {
            return GetCommand(new CommandIdentifier(commandSet, commandId));
        }

        public static ICommand GetCommand(CommandIdentifier identifier)
        {
            ICommand command;
            RegisterredCommands.TryGetValue(identifier, out command);
            return command;
        }
    }

    public struct CommandIdentifier
    {
        public Guid CommandSet { get; }
        public int CommandId { get; }

        public CommandIdentifier(Guid commandSet, int commandId)
        {
            CommandSet = commandSet;
            CommandId = commandId;
        }
    }
}