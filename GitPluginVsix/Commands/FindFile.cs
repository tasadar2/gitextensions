using System;
using System.Threading;
using EnvDTE;
using GitPluginVsix.Commands.Support;
using GitPluginVsix.Git;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitPluginVsix.Commands
{
    internal class FindFile : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.SolutionExplorerFileItem;

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