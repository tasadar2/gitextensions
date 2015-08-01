using System;
using Microsoft.VisualStudio.Shell;

namespace GitPluginVsix.Commands.Support
{
    internal abstract class Command : ICommand
    {
        public Package Package { get; set; }
        protected IServiceProvider ServiceProvider => Package;

        public abstract void OnCommand(object sender, EventArgs o);
    }
}