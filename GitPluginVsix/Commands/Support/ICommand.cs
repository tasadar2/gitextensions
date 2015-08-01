using System;
using Microsoft.VisualStudio.Shell;

namespace GitPluginVsix.Commands.Support
{
    public interface ICommand
    {
        Package Package { get; set; }
        void OnCommand(object sender, EventArgs o);
    }
}