using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HeBianGu.General.WpfControlLib;
using HeBianGu.Installer.Default.Models;
using HeBianGu.Installer.Default.ViewModels;

namespace HeBianGu.Installer.Default.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class InstallView : DialogWindowBase
    {

        public InstallView()
        {
            this.InitializeComponent();
            this.DataContext = InstallViewModel.GetViewModel();
            this.Closing += InstallView_Closing;
        }


        void InstallView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (InstallViewModel.GetViewModel().State == InstallState.Applying)
            {
                e.Cancel = true;
                return;
            }
            else
                CustomBootstrapperApplication.Dispatcher.InvokeShutdown();
        }


        //private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DragMove();
        //}
    }
}
