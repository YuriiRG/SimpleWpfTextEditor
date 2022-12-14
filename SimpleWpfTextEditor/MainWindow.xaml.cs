using SimpleWpfTextEditor.Data;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleWpfTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private readonly ApplicationData Data = new(new SettingsWriter());
        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new(Data.Locale);
            InitializeComponent();
            DataContext = Data;
            UpdateLanguageMenuItems();
        }

        private void UpdateLanguageMenuItems()
        {
            foreach (var langOption in MenuItemLangs.Items)
            {
                ((MenuItem)langOption).IsChecked = false;
            }
            ((MenuItem)MenuItemLangs.Items[Data.GetLocaleIndex()]).IsChecked = true;
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e) =>
            FileService.OpenFile(Data);

        private void SaveFile(object sender, ExecutedRoutedEventArgs e) =>
            FileService.SaveFile(Data);

        private void SaveFileAs(object sender, ExecutedRoutedEventArgs e) =>
            FileService.SaveFileAs(Data);

        private void IsAnyFileOpened(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = (Data.CurrentFileState != FileStates.NoFile);

        private void OpenChangeFontDialog(object sender, ExecutedRoutedEventArgs e) =>
            new FontDialog(Data).Show();

        private void ChangeFontSizeWithMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
                Data.FontSize++;

            else if (e.Delta < 0)
                Data.FontSize--;
        }

        private void ReloadCurrentFile(object sender, ExecutedRoutedEventArgs e)
        {
            FileService.ReloadCurrentFile(Data);
        }

        private void QuitApp(object sender, ExecutedRoutedEventArgs e) => Close();

        private void OpenRecentFile(object sender, ExecutedRoutedEventArgs e)
        {
            FileService.OpenRecentFile(Data, (string)e.Parameter);
        }

        private void ClearRecentFiles(object sender, ExecutedRoutedEventArgs e) =>
            Data.RecentFilesClear();

        private void OpenSearchDialog(object sender, ExecutedRoutedEventArgs e) =>
            new SearchDialog(SelectText, Data).Show();

        private void SelectText(int position, int length)
        {
            try
            {
                MainTextBox.Focus();
                MainTextBox.Select(position, length);
            }
            catch
            {
                MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                return;
            }
        }

        private void MainTextBox_LostFocus(object sender, RoutedEventArgs e) =>
            e.Handled = true;

        private void Window_Activated(object sender, System.EventArgs e) =>
            MainTextBox.Focus();

        private void Window_Deactivated(object sender, System.EventArgs e) =>
            MenuItemFile.Focus();

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (Data.GetLocaleIndex() == MenuItemLangs.Items.IndexOf(sender))
            {
                UpdateLanguageMenuItems();
                return;
            }
            if (!FileService.UnsavedFileMessage(Data))
            {
                UpdateLanguageMenuItems();
                return;
            }

            foreach (var langOption in MenuItemLangs.Items)
            {
                ((MenuItem)langOption).IsChecked = false;
            }
            ((MenuItem)sender).IsChecked = true;
            
            string locale;
            switch (((MenuItem)sender).Header)
            {
                case "English":
                    locale = "en";
                    break;
                case "Русский":
                    locale = "ru-RU";
                    break;
                default:
                    MessageBox.Show(Properties.Resources.GeneralError);
                    return;
            }
            Data.Locale = locale;

            new MainWindow().Show();
            Close();
        }

        private void ResetSettings(object sender, ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.ResetSettingsConfirmation,
                                         Properties.Resources.Confirmation,
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Data.ResetSettings();
                new MainWindow().Show();
                Close();
            }
        }
    }
}