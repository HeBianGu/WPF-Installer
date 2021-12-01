using System;
using System.Windows.Threading;
using HeBianGu.Base.WpfBase;
using HeBianGu.General.WpfControlLib;
using HeBianGu.Installer.Default.Models;
using HeBianGu.Installer.Default.ViewModels;
using HeBianGu.Installer.Default.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace HeBianGu.Installer.Default
{
    public class CustomBootstrapperApplication : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; set; }
        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            var model = BootstrapperApplicationModel.GetBootstrapperAppModel(this);
            var view = new InstallView();

            model.SetWindowHandle(view);

            this.Engine.Detect();

            view.Show();
            Dispatcher.Run();
            this.Engine.Quit(model.FinalResult);
        }
    }
}
