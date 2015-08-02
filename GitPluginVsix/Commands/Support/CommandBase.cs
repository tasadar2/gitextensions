using System;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using GitPluginVsix.Git;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Constants = EnvDTE.Constants;

namespace GitPluginVsix.Commands.Support
{
    internal abstract class CommandBase : ICommand
    {
        public Package Package { get; set; }
        protected IServiceProvider ServiceProvider => Package;
        protected abstract CommandTarget SupportedTargets { get; }

        public abstract void OnExecute(SelectedItem item, string fileName);

        public virtual void OnCommand(object sender, EventArgs e)
        {
            try
            {
                var dte = Package.GetGlobalService(typeof(SDTE)) as DTE;

                if (dte != null)
                {
                    if (!false)
                    {
                        var activeDocument = dte.ActiveDocument;

                        if (activeDocument?.ProjectItem == null)
                        {
                            // no active document - try solution target
                            if (dte.Solution.IsOpen && IsTargetSupported(CommandTarget.Solution))
                            {
                                OnExecute(null, dte.Solution.FullName);
                            }
                            // solution (or not supported) - try empty target
                            else if (IsTargetSupported(CommandTarget.Empty))
                            {
                                OnExecute(null, null);
                            }

                            return;
                        }

                        var fileName = activeDocument.ProjectItem.FileNames[1];

                        var selectedItem = dte.SelectedItems
                                              .Cast<SelectedItem>()
                                              .FirstOrDefault(solutionItem => solutionItem.ProjectItem != null && solutionItem.ProjectItem.FileNames[1] == fileName);

                        OnExecute(selectedItem, fileName);
                    }

                    if (dte.SelectedItems.Count == 0)
                    {
                        // nothing is selected, so if we have current solution and command supports that target, execute command on whole solution
                        if (dte.Solution.IsOpen && IsTargetSupported(CommandTarget.Solution))
                            OnExecute(null, dte.Solution.FullName);

                        // there is no opened solution, so try execute command for empty target
                        if (IsTargetSupported(CommandTarget.Empty))
                            OnExecute(null, null);

                        return;
                    }

                    foreach (SelectedItem solutionItem in dte.SelectedItems)
                    {
                        ExecuteOnSolutionItem(solutionItem, dte);
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void ExecuteOnSolutionItem(SelectedItem solutionItem, DTE application)
        {
            if (solutionItem.ProjectItem != null && IsTargetSupported(GetProjectItemTarget(solutionItem.ProjectItem)))
            {
                //Unfortunaly FileNames[1] is not supported by .net 3.5
                OnExecute(solutionItem, solutionItem.ProjectItem.get_FileNames(1));
                return;
            }

            if (solutionItem.Project != null && IsTargetSupported(CommandTarget.Project))
            {
                OnExecute(solutionItem, solutionItem.Project.FullName);
                return;
            }

            if (application.Solution.IsOpen && IsTargetSupported(CommandTarget.Solution))
            {
                OnExecute(solutionItem, application.Solution.FullName);
                return;
            }

            if (IsTargetSupported(CommandTarget.Empty))
            {
                OnExecute(solutionItem, null);
                return;
            }

            MessageBox.Show("You need to select a file or project to use this function.", "Git Extensions", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static CommandTarget GetProjectItemTarget(ProjectItem projectItem)
        {
            switch (projectItem.Kind.ToUpper())
            {
                case Constants.vsProjectItemKindPhysicalFile:
                    return CommandTarget.File;
                case Constants.vsProjectItemKindVirtualFolder:
                    return CommandTarget.VirtualFolder;
                case Constants.vsProjectItemKindPhysicalFolder:
                    return CommandTarget.PhysicalFolder;
                default:
                    return CommandTarget.Any;
            }
        }

        private bool IsTargetSupported(CommandTarget commandTarget)
        {
            return (SupportedTargets & commandTarget) == commandTarget;
        }

        protected static void RunGitEx(string command, string filename)
        {
            GitCommands.RunGitEx(command, filename);
        }

        [Flags]
        protected enum CommandTarget
        {
            /// <summary>
            /// Solution file selected in solution explorer.
            /// </summary>
            Solution = 1,

            /// <summary>
            /// Project file selected in solution explorer.
            /// </summary>
            Project = 2,

            /// <summary>
            /// Physical folder selected in solution explorer.
            /// </summary>
            PhysicalFolder = 4,

            /// <summary>
            /// Project item file selected in solution explorer.
            /// </summary>
            File = 8,

            /// <summary>
            /// Virtual folder selected in solution explorer.
            /// </summary>
            VirtualFolder = 16,

            /// <summary>
            /// Nothing is selected, no current solution.
            /// </summary>
            Empty = 32,

            /// <summary>
            /// Any solution explorer item that presented by physical file.
            /// </summary>
            SolutionExplorerFileItem = Solution | Project | File,

            /// <summary>
            /// Any target including empty.
            /// </summary>
            Any = SolutionExplorerFileItem | PhysicalFolder | VirtualFolder | Empty
        }


    }

}