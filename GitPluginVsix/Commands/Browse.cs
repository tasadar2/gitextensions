using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal sealed class Browse : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.Any;

        public override void OnExecute(SelectedItem selectedItem, string fileName)
        {
            RunGitEx("browse", fileName);
        }

    }
}