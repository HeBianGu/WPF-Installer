using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HeBianGu.Installer.Default;
using HeBianGu.Installer.Default.Models;
using HeBianGu.Installer.Default.Views;

namespace HeBianGu.Application.InstallerTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DefaultBootstrapperApplication application = new DefaultBootstrapperApplication();

            var model = BootstrapperApplicationModel.GetBootstrapperAppModel(application);
            var view = new InstallView();

            model.SetWindowHandle(view);

            application.Engine?.Detect();

            view.Show();
            //Dispatcher.Run();
            application.Engine?.Quit(model.FinalResult);
        }
    }
}
