using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class Commit : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            item?.DTE.ExecuteCommand("File.SaveAll");

            RunGitEx("commit", fileName);
        }
    }
}