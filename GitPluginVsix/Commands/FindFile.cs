using System;
using System.Threading;
using EnvDTE;
using GitPluginVsix.Commands.Support;
using GitPluginVsix.Git;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitPluginVsix.Commands
{
    internal class FindFile : CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0410;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.SolutionExplorerFileItem;

        public override void OnCommand(object sender, EventArgs e)
        {
            try
            {
                var dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
                if (dte != null)
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        string file = GitCommands.RunGitExWait("searchfile", dte.Solution.FullName);
                        if (string.IsNullOrEmpty(file?.Trim()))
                            return;
                        dte.ExecuteCommand("File.OpenFile", file);
                    });
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public override void OnExecute(SelectedItem item, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}