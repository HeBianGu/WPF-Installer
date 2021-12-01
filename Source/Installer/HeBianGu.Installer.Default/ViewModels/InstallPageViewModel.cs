
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeBianGu.Base.WpfBase;
using HeBianGu.Installer.Default.Models;
using HeBianGu.Installer.Default.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace HeBianGu.Installer.Default.ViewModels
{
    public class InstallPageViewModel : BaseViewModel
    {
        private static string SoftWareName = "wpfapptopackage";
        private bool createShortCut;
        private string installFolder;
        private Visibility selectFileVisibility;
        private RelayCommand BrowseCommand;
        private RelayCommand InstallCommand;
        private RelayCommand CloseCommand;
        private RelayCommand ShowSelecFileCommand;

        /// <summary>
        /// InstallViewModel单例的引用，用来修改InstallViewModel.State
        /// 以实时根据当前状态更改显示的界面内容
        /// </summary>
        private InstallViewModel installViewModel
        {
            get { return InstallViewModel.GetViewModel(); }
        }
        /// <summary>
        /// Model单例的引用，用来定义事件触发执行什么操作
        /// 同时通过该引用的某些方法来执行安装功能
        /// </summary>
        private BootstrapperApplicationModel BootstrapperModel
        {
            get
            {
                return BootstrapperApplicationModel.GetBootstrapperAppModel();
            }
        }
        public InstallPageViewModel()
        {
            InitialCommand();
            SeleFileVisibility = Visibility.Collapsed;
            CreateShortCut = true;
            InstallFolder = @"C:\Program Files (x86)\DeepGlin\" + SoftWareName;
            WireUpEventHandlers();
        }


        /// <summary>
        /// 自定义安装路径的控件组是否显示
        /// </summary>
        public Visibility SeleFileVisibility
        {
            get { return selectFileVisibility; }
            set
            {
                selectFileVisibility = value;
                OnPropertyChanged("SeleFileVisibility");
            }
        }
        /// <summary>
        /// 是否创建桌面快捷方式
        /// </summary>
        public bool CreateShortCut
        {
            get { return createShortCut; }
            set
            {
                createShortCut = value;
                OnPropertyChanged("CreateShortCut");
                this.SetBurnVariable("CreateShortCut", createShortCut.ToString());
            }
        }
        /// <summary>
        /// 自定义安装路径
        /// 在用户选择的路径后再创建一个当前软件名称的文件夹，
        /// 使安装不混乱在根目录中
        /// </summary>
        public string InstallFolder
        {
            get { return installFolder; }
            set
            {
                try
                {
                    if (value != installFolder && ValidDir(value))
                    {
                        string[] para = value.Split('\\');
                        bool hassoftwarename = false;
                        foreach (string pa in para)
                        {
                            if (pa == SoftWareName)
                                hassoftwarename = true;
                        }
                        if (hassoftwarename)
                            installFolder = value;
                        else
                            installFolder = value + "\\" + SoftWareName;
                        OnPropertyChanged("InstallFolder");
                        this.SetBurnVariable("InstallFolder", installFolder);
                    }
                }
                catch
                {
                    installFolder = value;
                }
            }
        }

        /// <summary>
        /// 界面四个按钮的单击事件
        /// </summary>

        public ICommand btn_browse
        {
            get { return BrowseCommand; }
        }
        public ICommand btn_install
        {
            get { return InstallCommand; }
        }
        public ICommand btn_cancel
        {
            get { return CloseCommand; }
        }
        public ICommand btn_show
        {
            get { return ShowSelecFileCommand; }
        }
        /// <summary>
        /// 初始化按钮点击命令要调用的函数
        /// </summary>
        private void InitialCommand()
        {
            BrowseCommand = new RelayCommand(Browse, IsValid);
            InstallCommand = new RelayCommand(Install, IsValid);
            CloseCommand = new RelayCommand(Close, IsValid);
            ShowSelecFileCommand = new RelayCommand(Show, IsValid);
        }
        /// <summary>
        /// 打开选择安装路径窗口并获取用户选择的路径
        /// </summary>
        public void Browse(object o)
        {
            var folderBrowserDialog = new FolderBrowserDialog { SelectedPath = InstallFolder };

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InstallFolder = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// 开始安装
        /// 调用PlanAction()方法而不是ApplyAction()
        /// 此时只是开始执行配置安装信息，并不会执行安装进程
        /// </summary>
        public void Install(object o)
        {
            this.BootstrapperModel.PlanAction(LaunchAction.Install);
        }
        /// <summary>
        /// 取消安装，关闭安装进程
        /// </summary>
        public void Close(object o)
        {
            installViewModel.State = InstallState.Cancelled;
            CustomBootstrapperApplication.Dispatcher.InvokeShutdown();
        }
        /// <summary>
        /// 显示/关闭自定义安装界面
        /// </summary>
        public void Show(object o)
        {
            if (SeleFileVisibility == Visibility.Collapsed)
                SeleFileVisibility = Visibility.Visible;
            else
                SeleFileVisibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 当安装进程开始时触发事件
        /// 将当前状态更改位Applying
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            this.installViewModel.State = InstallState.Applying;
        }
        /// <summary>
        /// 开始安装时调用的方法位PlanAction()
        /// 该方法执行完成之后触发本事件
        /// 事件中调用ApplyAction()方法开始执行安装进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (installViewModel.State == InstallState.Cancelled)
            {
                CustomBootstrapperApplication.Dispatcher
                  .InvokeShutdown();
                return;
            }
            this.BootstrapperModel.ApplyAction();
        }

        /// <summary>
        /// 注册BootstrapperApplication的两个事件
        /// </summary>
        private void WireUpEventHandlers()
        {
            if (this.BootstrapperModel == null) return;

            this.BootstrapperModel.BootstrapperApplication.PlanComplete += this.PlanComplete;
            this.BootstrapperModel.BootstrapperApplication.ApplyBegin += this.ApplyBegin;
        }

        /// <summary>
        /// pathj是否是正确的文件夹路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ValidDir(string path)
        {
            try
            {
                string p = new DirectoryInfo(path).FullName;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 向Burn传递用户参数
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public void SetBurnVariable(string variableName, string value)
        {
            if (this.BootstrapperModel == null) return;

            this.BootstrapperModel.SetBurnVariable(variableName, value);
        }
        public bool IsValid(object o)
        {
            return true;
        }
    }
}
