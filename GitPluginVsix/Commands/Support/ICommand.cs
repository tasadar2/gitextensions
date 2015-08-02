using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.Shell;

namespace GitPluginVsix.Commands.Support
{
    public interface ICommand
    {
        Guid CommandSet { get; }
        int CommandId { get; }
        Package Package { get; set; }

        void OnCommand(object sender, EventArgs o);
        bool IsEnabled();
    }
}