using System;
using System.IO;
using System.Linq;
using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class CloneRepo : CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0110;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && Path.GetInvalidPathChars().Any(fileName.Contains))
            {
                fileName = "";
            }

            var directoryName = Path.GetDirectoryName(fileName);
            RunGitEx("clone", directoryName);
        }
    }
}