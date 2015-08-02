using System;
using EnvDTE;
using GitPluginVsix.Commands.Support;
using GitPluginVsix.Git;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitPluginVsix.Commands
{
    internal class Commit : CommandBase, ICommand
    {
        public static Guid CommandSet { get; } = GitPlugin.CommandCmdSet;
        public static int CommandId { get; } = 0x0200;

        Guid ICommand.CommandSet { get; } = CommandSet;
        int ICommand.CommandId { get; } = CommandId;

        private static DateTime _lastBranchCheck;
        private static string _lastFile;
        private static bool _showCurrentBranch;
        private static string _lastUpdatedCaption;

        protected override CommandTarget SupportedTargets { get; } = CommandTarget.Any;

        public Commit()
        {
            if (_lastFile == null)
            {
                _lastFile = string.Empty;
            }

            _showCurrentBranch = GitCommands.GetShowCurrentBranchSetting();
        }

        public override void OnExecute(SelectedItem item, string fileName)
        {
            item?.DTE.ExecuteCommand("File.SaveAll");

            RunGitEx("commit", fileName);
        }

        public override bool IsEnabled()
        {
            var enabled = base.IsEnabled();

            var dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
            if (dte != null)
            {
                var fileName = GetSelectedFile(dte);

                if (_showCurrentBranch && (fileName != _lastFile || DateTime.Now - _lastBranchCheck > new TimeSpan(0, 0, 0, 1, 0)))
                {
                    var newCaption = "Commit";
                    if (enabled)
                    {
                        string head = GitCommands.GetCurrentBranch(fileName);
                        if (!string.IsNullOrEmpty(head))
                        {
                            string headShort;
                            if (head.Length > 27)
                            {
                                headShort = "..." + head.Substring(head.Length - 23);
                            }
                            else
                            {
                                headShort = head;
                            }

                            newCaption = "Commit (" + headShort + ")";
                        }
                    }

                    // This guard required not only for perfromance, but also for prevent StackOverflowException.
                    // IDE.QueryStatus -> Commit.IsEnabled -> Plugin.UpdateCaption -> IDE.QueryStatus ...
                    if (_lastUpdatedCaption != newCaption)
                    {
                        _lastUpdatedCaption = newCaption;

                        // try apply new caption (operation can fail)
                        if (!ChangeCommandCaption(dte, "GitExtensions", "Commit changes", newCaption))
                        {
                            _lastUpdatedCaption = null;
                        }
                    }

                    _lastBranchCheck = DateTime.Now;
                    _lastFile = fileName;
                }
            }

            return enabled;
        }

        private static string GetSelectedFile(_DTE application)
        {
            foreach (SelectedItem sel in application.SelectedItems)
            {
                if (sel.ProjectItem != null)
                {
                    if (sel.ProjectItem.FileCount > 0)
                    {
                        //Unfortunaly FileNames[1] is not supported by .net 3.5
                        return sel.ProjectItem.get_FileNames(1);
                    }
                }
                else if (sel.Project != null)
                {
                    return sel.Project.FullName;
                }
            }

            if (application.Solution.IsOpen)
            {
                return application.Solution.FullName;
            }

            return string.Empty;
        }

    }
}