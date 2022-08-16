using Microsoft.Win32;
using SimpleWpfTextEditor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleWpfTextEditor
{
    public static class FileService
    {
        private const string PlainTextFilterString = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*";

        public static void OpenFile(ApplicationData data)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = PlainTextFilterString
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (!UnsavedFileMessage(data))
                {
                    return;
                }
                data.CurrentFilePath = openFileDialog.FileName;
                data.Text = File.ReadAllText(data.CurrentFilePath);
                data.EventHappened(FileEvents.FileOpened);
            }
        }

        public static void ReloadCurrentFile(ApplicationData data)
        {
            if (!FileService.UnsavedFileMessage(data))
            {
                return;
            }
            data.Text = File.ReadAllText(data.CurrentFilePath);
            data.EventHappened(FileEvents.FileOpened);
        }

        public static void SaveFile(ApplicationData data)
        {
            data.EventHappened(FileEvents.FileSaved);
            data.Text = data.Text.Replace("\r\n", data.NewLine);
            File.WriteAllText(data.CurrentFilePath, data.Text);
        }

        public static void SaveFileAs(ApplicationData data)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = PlainTextFilterString
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                data.CurrentFilePath = saveFileDialog.FileName;
                SaveFile(data);
            }
        }

        public static bool UnsavedFileMessage(ApplicationData data)
        {
            if (data.CurrentFileState == FileStates.ChangedFile)
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
    }
}
