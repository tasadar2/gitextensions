using System.IO;
using System.Linq;
using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal class CreateNewRepo : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.Any;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && Path.GetInvalidPathChars().Any(fileName.Contains))
                fileName = "";
            var directoryName = Path.GetDirectoryName(fileName);
            RunGitEx("init", directoryName);
        }
    }
}