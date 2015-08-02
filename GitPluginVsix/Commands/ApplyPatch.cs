using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class ApplyPatch : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.SolutionExplorerFileItem;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("applypatch", fileName);
        }
    }
}