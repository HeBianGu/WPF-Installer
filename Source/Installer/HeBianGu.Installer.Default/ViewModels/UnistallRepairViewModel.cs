using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeBianGu.Base.WpfBase;
using HeBianGu.Installer.Default.Models;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace HeBianGu.Installer.Default.ViewModels
{
    public class UnistallRepairViewModel
    {
        private InstallViewModel installViewModel
        {
            get { return InstallViewModel.GetViewModel(); }
        }
        private BootstrapperApplicationModel BootstrapperModel
        {
            get
            { return BootstrapperApplicationModel.GetBootstrapperAppModel(); }
        }
        private RelayCommand CloseCommand;
        private RelayCommand RepairCommand;
        private RelayCommand UninstallCommand;
        public ICommand btn_close
        {
            get { return CloseCommand; }
        }
        public ICommand btn_repair
        {
            get { return RepairCommand; }
        }
        public ICommand btn_uninstall
        {
            get { return UninstallCommand; }
        }

        public UnistallRepairViewModel()
        {
            UninstallCommand = new RelayCommand(Uninstall, IsValid);
            RepairCommand = new RelayCommand(Repair, IsValid);
            CloseCommand = new RelayCommand(Close, IsValid);
        }
        public void Repair(object o)
        {
            BootstrapperModel.PlanAction(LaunchAction.Repair);
        }
        public void Uninstall(object o)
        {
            BootstrapperModel.PlanAction(LaunchAction.Uninstall);
        }
        public void Close(object o)
        {
            CustomBootstrapperApplication.Dispatcher.InvokeShutdown();
        }
        public bool IsValid(object o)
        {
            return true;
        }

    }
}
