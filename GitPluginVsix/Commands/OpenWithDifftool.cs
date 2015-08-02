﻿using EnvDTE;
using GitPluginVsix.Commands.Support;

namespace GitPluginVsix.Commands
{
    internal sealed class OpenWithDiftool : CommandBase
    {
        protected override CommandTarget SupportedTargets => CommandTarget.SolutionExplorerFileItem;

        public override void OnExecute(SelectedItem item, string fileName)
        {
            RunGitEx("difftool", fileName);
        }

    }
}
