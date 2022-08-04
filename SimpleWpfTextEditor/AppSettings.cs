using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWpfTextEditor
{
    public class AppSettings
    {
        public ObservableCollection<string> RecentFiles { get; set; } = new();
        public string FontFamily { get; set; } = "Consolas";
        public double FontSize { get; set; } = 14;
        public bool WrapText { get; set; } = true;
    }
}
