using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeBianGu.Base.WpfBase;

namespace HeBianGu.Installer.Default
{
    internal class InstallerConfig : LazyInstance<InstallerConfig>
    {
        public string Maufacturer { get; set; } = "HeBianGu";

        public string ProductName { get; set; } = "ShellOffice";

        public string Version { get; set; } = "1.0.0";

        public string Description { get; set; }

        internal string ProgramFilesFolder => "Program Files (x86)";

        internal string DefaultLocation => $"C:\\{ProgramFilesFolder}\\{Maufacturer}\\{ProductName}";

        internal string Bit3264 => @"SOFTWARE\WOW6432Node\Microsoft\" + Maufacturer; 
        internal string Bit32 => @"SOFTWARE\Microsoft\" + Maufacturer;

        internal string MyInstellerName = "HeBianGu.Setup.Demo.msi";

        public string Contract { get; set; } = "QQ:908293466";
    }
}
