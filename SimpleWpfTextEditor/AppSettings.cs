using System.Collections.ObjectModel;

namespace SimpleWpfTextEditor
{
    public class AppSettings
    {
        public ObservableCollection<string> RecentFiles { get; set; } = new();
        public string FontFamily { get; set; } = "Consolas";
        public double FontSize { get; set; } = 14;
        public bool WrapText { get; set; } = true;
        public string Locale { get; set; } = "en";
    }
}
