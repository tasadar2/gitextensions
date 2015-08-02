﻿using System;
using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal sealed class About : CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0620;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.Any;

        public override void OnExecute(SelectedItem selectedItem, string fileName)
        {
            RunGitEx("about", fileName);
        }
    }
}