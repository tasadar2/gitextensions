using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class CheckoutBranch : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("checkoutbranch", fileName);
        }
    }
}