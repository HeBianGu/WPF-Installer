using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HeBianGu.Installer.Default.ViewModels;

namespace HeBianGu.Installer.Default.Views
{
    /// <summary>
    /// Interaction logic for CancelPage.xaml
    /// </summary>
    public partial class CancelPage : UserControl
    {
        public CancelPage()
        {
            InitializeComponent();
            this.DataContext = new CancelViewModel();
        }
    }
}
