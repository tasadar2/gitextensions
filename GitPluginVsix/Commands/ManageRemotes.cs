﻿using System;
using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class ManageRemotes : CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0240;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("remotes", fileName);
        }
    }
}