﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
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
        private readonly FileStateFSM fileStateFSM = new();
        private const string PlainTextFilterString = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*";
        private readonly ApplicationData Data = new();
        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new(Data.Locale);
            InitializeComponent();
            DataContext = Data;
            UpdateLanguageMenuItems();
            UpdateWindowTitle();
        }

        private void UpdateLanguageMenuItems()
        {
            foreach (var langOption in MenuItemLangs.Items)
            {
                ((MenuItem)langOption).IsChecked = false;
            }
            ((MenuItem)MenuItemLangs.Items[Data.GetLocaleIndex()]).IsChecked = true;
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
            Data.UpdateCounters();
        }

        private void UpdateWindowTitle()
        {
            switch (fileStateFSM.State)
            {
                case FileStates.NoFile:
                    Data.WindowTitle = Properties.Resources.WindowTitleNoFile;
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
                string unsavedFileMessage = Properties.Resources.UnsavedChanges;
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

        private void UpdateRecentFiles(string newFilePath)
        {
            if (Data.RecentFiles.Contains(newFilePath))
            {
                Data.RecentFilesRemoveAt(Data.RecentFiles.IndexOf(newFilePath));
            }
            Data.RecentFilesInsert(0, newFilePath);
            if (Data.RecentFiles.Count > 10)
            {
                Data.RecentFilesRemoveAt(Data.RecentFiles.Count - 1);
            }
        }

        private void ClearRecentFiles(object sender, ExecutedRoutedEventArgs e)
        {
            Data.RecentFilesClear();
        }

        private void OpenSearchDialog(object sender, ExecutedRoutedEventArgs e)
        {
            SearchDialog dialog = new(SelectText, Data);
            dialog.Show();
        }
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
                                    "Notification",
                                    MessageBoxButton.OK);
                return;
            }
        }

        private void MainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            MainTextBox.Focus();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            MenuItemFile.Focus();
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (Data.GetLocaleIndex() == MenuItemLangs.Items.IndexOf(sender))
            {
                UpdateLanguageMenuItems();
                return;
            }
            if (!UnsavedFileMessage())
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
    }
}