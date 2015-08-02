﻿using System;
using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class CherryPick: CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0550;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.SolutionExplorerFileItem;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("cherry", fileName);
        }
    }
}