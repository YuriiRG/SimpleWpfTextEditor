using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace SimpleWpfTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileStateFSM fileStateFSM = new();
        private const string PlainTextFilterString = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*";
        private readonly ApplicationData Data;
        public MainWindow()
        {
            Data = SettingsWriter.Read();
            InitializeComponent();
            DataContext = Data;
            UpdateRecentFiles();
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = PlainTextFilterString
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (!UnsavedFileMessage())
                {
                    return;
                }
                Data.CurrentFilePath = openFileDialog.FileName;
                Data.Text = File.ReadAllText(openFileDialog.FileName);
                fileStateFSM.EventHappened(FileEvents.FileOpened);
                UpdateWindowTitle();
                UpdateRecentFiles(Data.CurrentFilePath);
            }
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e)
        {
            File.WriteAllText(Data.CurrentFilePath, Data.Text);
            fileStateFSM.EventHappened(FileEvents.FileSaved);
            UpdateWindowTitle();
        }

        private void SaveFileAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = PlainTextFilterString
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                Data.CurrentFilePath = saveFileDialog.FileName;
                File.WriteAllText(Data.CurrentFilePath, Data.Text);
                fileStateFSM.EventHappened(FileEvents.FileSaved);
                UpdateWindowTitle();
            }
        }

        private void IsAnyFileOpened(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (fileStateFSM.State != FileStates.NoFile);
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fileStateFSM.EventHappened(FileEvents.FileChanged);
            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            switch (fileStateFSM.State)
            {
                case FileStates.NoFile:
                    Data.WindowTitle = "No file opened";
                    break;
                case FileStates.FileNoChanges:
                    Data.WindowTitle = Data.CurrentFilePath;
                    break;
                case FileStates.ChangedFile:
                    Data.WindowTitle = Data.CurrentFilePath + "*";
                    break;
            }
        }

        private void OpenChangeFontDialog(object sender, ExecutedRoutedEventArgs e)
        {
            FontDialog dialog = new(Data);
            dialog.Show();
        }

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
            if (!UnsavedFileMessage())
            {
                return;
            }
            Data.Text = File.ReadAllText(Data.CurrentFilePath);
            fileStateFSM.EventHappened(FileEvents.FileOpened);
            UpdateWindowTitle();
        }

        private bool UnsavedFileMessage()
        {
            if (fileStateFSM.State == FileStates.ChangedFile)
            {
                string unsavedFileMessage =
                    "Unsaved changes will be lost. Are you sure you want to open this file?";
                var result = MessageBox.Show(unsavedFileMessage,
                                             "Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            return true;
        }

        private void QuitApp(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OpenRecentFile(object sender, ExecutedRoutedEventArgs e)
        {
            Data.CurrentFilePath = (string)e.Parameter;
            Data.Text = File.ReadAllText(Data.CurrentFilePath);
            fileStateFSM.EventHappened(FileEvents.FileOpened);
            UpdateWindowTitle();
            UpdateRecentFiles(Data.CurrentFilePath);
        }

        private void UpdateRecentFiles(string? newFilePath = null)
        {
            if (newFilePath != null)
            {
                Data.RecentFiles.Insert(0, newFilePath!);
                if (Data.RecentFiles.Count > 10)
                {
                    Data.RecentFiles.RemoveAt(Data.RecentFiles.Count - 1);
                }
                SettingsWriter.Save(Data);
            }
            Data.IsRecentFilesNotEmpty = !(Data.RecentFiles.Count == 0);
        }
    }
}