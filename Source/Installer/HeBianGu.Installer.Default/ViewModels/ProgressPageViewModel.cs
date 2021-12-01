using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeBianGu.Base.WpfBase;
using HeBianGu.Installer.Default.Models;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace HeBianGu.Installer.Default.ViewModels
{
    public class ProgressPageViewModel : NotifyPropertyChanged
    {
        private InstallViewModel installViewModel
        {
            get { return InstallViewModel.GetViewModel(); }
        }
        private BootstrapperApplicationModel BootstrapperModel
        {
            get
            {
                return BootstrapperApplicationModel.GetBootstrapperAppModel();
            }
        }
        private int cacheProgress;
        private int executeProgress;
        private int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }


        private RelayCommand RollBackCommand;

        public ICommand btn_cancel
        {
            get { return RollBackCommand; }
        }
        public ProgressPageViewModel()
        {
            RollBackCommand = new RelayCommand(RollBack, IsValid);

            if (this.BootstrapperModel == null) return;

            this.BootstrapperModel.BootstrapperApplication.ApplyComplete += this.ApplyComplete;
            this.BootstrapperModel.BootstrapperApplication.ExecutePackageBegin += this.ExecutePackageBegin;
            this.BootstrapperModel.BootstrapperApplication.ExecutePackageComplete += this.ExecutePackageComplete;
            this.BootstrapperModel.BootstrapperApplication.CacheAcquireProgress +=
               (sender, args) =>
               {
                   this.cacheProgress = args.OverallPercentage;
                   this.Progress =
                      (this.cacheProgress + this.executeProgress) / 2;
               };
            this.BootstrapperModel.BootstrapperApplication.ExecuteProgress +=
               (sender, args) =>
               {
                   this.executeProgress = args.OverallPercentage;
                   this.Progress =
                      (this.cacheProgress + this.executeProgress) / 2;
               };
            this.BootstrapperModel.BootstrapperApplication.ExecuteMsiMessage += BootstrapperApplication_ExecuteMsiMessage;
        }

        private void BootstrapperApplication_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            lock (this)
            {
                if (e.MessageType == InstallMessage.ActionStart)
                {
                    this.Message = e.Message;
                }
            }
        }
        protected void ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            if (installViewModel.State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }

        protected void ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            if (installViewModel.State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }
        public void RollBack(object o)
        {
            installViewModel.State = InstallState.Cancelled;
            DefaultBootstrapperApplication.Dispatcher.InvokeShutdown();
        }
        public bool IsValid(object o)
        {
            return true;
        }
        protected void ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            this.BootstrapperModel.FinalResult = e.Status;
            installViewModel.State = InstallState.Applied;
        }
    }
}
