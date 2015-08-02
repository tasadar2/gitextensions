using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class Push : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("push", fileName);
        }
    }
}