using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeBianGu.Base.WpfBase;

namespace HeBianGu.Installer.Default.ViewModels
{
    public class CancelViewModel
    {
        public CancelViewModel()
        {
            CloseCommand = new RelayCommand(Close, IsValid);
        }
        private RelayCommand CloseCommand;

        public ICommand btn_close
        {
            get { return CloseCommand; }
        }

        public void Close(object o)
        {
            DefaultBootstrapperApplication.Dispatcher.InvokeShutdown();
        }
        public bool IsValid(object o)
        {
            return true;
        }
    }
}
