using Microsoft.Win32;
using SimpleWpfTextEditor.Data;
using System.IO;
using System.Windows;

namespace SimpleWpfTextEditor
{
    public static class FileService
    {
        private const string PlainTextFilterString = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*";

        public static void OpenFile(AppViewModel viewModel)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = PlainTextFilterString
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (!UnsavedFileMessage(viewModel))
                {
                    return;
                }
                viewModel.CurrentFilePath = openFileDialog.FileName;
                viewModel.Text = File.ReadAllText(viewModel.CurrentFilePath);
                viewModel.EventHappened(FileEvents.FileOpened);
            }
        }

        public static void ReloadCurrentFile(AppViewModel viewModel)
        {
            if (!FileService.UnsavedFileMessage(viewModel))
            {
                return;
            }
            viewModel.Text = File.ReadAllText(viewModel.CurrentFilePath);
            viewModel.EventHappened(FileEvents.FileOpened);
        }

        public static void SaveFile(AppViewModel viewModel)
        {
            viewModel.EventHappened(FileEvents.FileSaved);
            string correctText = viewModel.Text.Replace("\r\n", viewModel.NewLine);
            File.WriteAllText(viewModel.CurrentFilePath, correctText);
        }

        public static void SaveFileAs(AppViewModel viewModel)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = PlainTextFilterString
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                viewModel.CurrentFilePath = saveFileDialog.FileName;
                SaveFile(viewModel);
            }
        }

        public static bool UnsavedFileMessage(AppViewModel viewModel)
        {
            if (viewModel.CurrentFileState == FileStates.ChangedFile)
            {
                string unsavedFileMessage = Properties.Resources.UnsavedChanges;
                var result = MessageBox.Show(unsavedFileMessage,
                                             Properties.Resources.Confirmation,
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static void OpenRecentFile(AppViewModel viewModel, string RecentFilePath)
        {
            viewModel.CurrentFilePath = RecentFilePath;
            viewModel.Text = File.ReadAllText(viewModel.CurrentFilePath);
            viewModel.EventHappened(FileEvents.FileOpened);
        }
    }
}
