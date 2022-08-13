using System.Windows.Input;

namespace SimpleWpfTextEditor
{
    static class CustomCommands
    {
        public static RoutedCommand ChangeFont { get; set; } =
            new RoutedCommand("ChangeFont", typeof(MainWindow));
        public static RoutedCommand ReloadFile { get; set; } =
            new RoutedCommand("ReloadFile", typeof(MainWindow));
        public static RoutedCommand Quit { get; set; } =
            new RoutedCommand("Quit", typeof(MainWindow));
        public static RoutedCommand OpenRecentFile { get; set; } =
            new RoutedCommand("OpenRecentFile", typeof(MainWindow));
        public static RoutedCommand ClearRecentFiles { get; set; } =
            new RoutedCommand("ClearRecentFiles", typeof(MainWindow));
        public static RoutedCommand ResetSettings { get; set; } =
            new RoutedCommand("ResetSettings", typeof(MainWindow));
    }
}
