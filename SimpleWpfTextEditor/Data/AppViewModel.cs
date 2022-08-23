using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SimpleWpfTextEditor.Data
{
    /// <summary>
    /// Application ViewModel. Contains all logic (computed properties) related to data.
    /// This class is MainWindow's DataContext.
    /// Also, it encapsulates saving settings with AppSettings and SettingsWriter.
    /// </summary>
    public class AppViewModel : ObservableObject, IAppViewModel
    {
        public RelayCommand ChangeFontCommand { get; }
        public RelayCommand ReloadFileCommand { get; }
        public RelayCommand QuitCommand { get; }
        public RelayCommand<object> OpenRecentFileCommand { get; }
        public RelayCommand ClearRecentFilesCommand { get; }
        public RelayCommand ResetSettingsCommand { get; }

        private readonly ISettingsWriter settingsWriter;
        private AppSettings settings;
        public AppViewModel(ISettingsWriter writer)
        {
            settingsWriter = writer;
            settings = settingsWriter.Read();

            ChangeFontCommand = new(OpenChangeFontDialog);
            ReloadFileCommand = new(ReloadCurrentFile, IsAnyFileOpened);

            OpenRecentFileCommand = new(OpenRecentFile);
            ClearRecentFilesCommand = new(RecentFilesClear);
        }


        private void OpenChangeFontDialog() =>
            new FontDialog(this).Show();

        private void ReloadCurrentFile() =>
            FileService.ReloadCurrentFile(this);

        private void OpenRecentFile(object? parameter)
        {
            ReloadFileCommand.NotifyCanExecuteChanged();
            FileService.OpenRecentFile(this, (string)parameter!);
        }
            

        private bool IsAnyFileOpened()
        {
            return (CurrentFileState != FileStates.NoFile);
        } 
            


        public void ResetSettings()
        {
            settingsWriter.Reset();
        }

        public ObservableCollection<string> RecentFiles
        {
            get
            {
                return settings.RecentFiles;
            }
            set
            {
                if (value != settings.RecentFiles)
                {
                    settings.RecentFiles = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsRecentFilesNotEmpty));
                    settingsWriter.Save(settings);
                }
            }
        }

        public void RecentFilesInsert(int index, string newRecentFile)
        {
            RecentFiles.Insert(index, newRecentFile);
            OnPropertyChanged(nameof(RecentFiles));
            OnPropertyChanged(nameof(IsRecentFilesNotEmpty));
            settingsWriter.Save(settings);
        }

        public int GetLocaleIndex()
        {
            return Locale switch
            {
                "en" => 0,
                "ru-RU" => 1,
                _ => 0,
            };
        }

        public void RecentFilesRemoveAt(int index)
        {
            RecentFiles.RemoveAt(index);
            OnPropertyChanged(nameof(RecentFiles));
            OnPropertyChanged(nameof(IsRecentFilesNotEmpty));
            settingsWriter.Save(settings);
        }

        public void RecentFilesClear()
        {
            RecentFiles.Clear();
            OnPropertyChanged(nameof(RecentFiles));
            OnPropertyChanged(nameof(IsRecentFilesNotEmpty));
            settingsWriter.Save(settings);
        }

        public string FontFamily
        {
            get
            {
                return settings.FontFamily;
            }
            set
            {
                if (value != settings.FontFamily)
                {
                    settings.FontFamily = value;
                    OnPropertyChanged();
                    settingsWriter.Save(settings);
                }
            }
        }

        public double FontSize
        {
            get
            {
                return settings.FontSize;
            }
            set
            {
                if (value != settings.FontSize)
                {
                    settings.FontSize = value;
                    OnPropertyChanged();
                    settingsWriter.Save(settings);
                }
            }
        }

        public bool WrapText
        {
            get
            {
                return settings.WrapText;
            }
            set
            {
                if (value != settings.WrapText)
                {
                    settings.WrapText = value;
                    OnPropertyChanged();
                    settingsWriter.Save(settings);
                }
            }
        }

        public string Locale
        {
            get
            {
                return settings.Locale;
            }
            set
            {
                if (value != settings.Locale)
                {
                    settings.Locale = value;
                    settingsWriter.Save(settings);
                }
            }
        }

        public string CurrentFilePath { get; set; } = string.Empty;

        private string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                    UpdateCounters();
                    EventHappened(FileEvents.FileChanged);
                }
            }
        }

        private FileStateFSM fileStateMachine = new();

        public FileStates CurrentFileState
        {
            get
            {
                return fileStateMachine.State;
            }
        }

        public void EventHappened(FileEvents newEvent)
        {
            fileStateMachine.EventHappened(newEvent);
            OnPropertyChanged(nameof(WindowTitle));
            OnPropertyChanged(nameof(CurrentFileState));
            if (newEvent == FileEvents.FileOpened)
            {
                UpdateNewLine();
                AddToRecentFiles(CurrentFilePath);
            }
        }

        private void AddToRecentFiles(string newFilePath)
        {
            if (RecentFiles.Contains(newFilePath))
            {
                RecentFilesRemoveAt(RecentFiles.IndexOf(newFilePath));
            }
            RecentFilesInsert(0, newFilePath);
            if (RecentFiles.Count > 10)
            {
                RecentFilesRemoveAt(RecentFiles.Count - 1);
            }
        }

        public string WindowTitle
        {
            get
            {
                return CurrentFileState switch
                {
                    FileStates.NoFile        => Properties.Resources.WindowTitleNoFile,
                    FileStates.FileNoChanges => CurrentFilePath,
                    FileStates.ChangedFile   => CurrentFilePath + "*",
                    _                        => Properties.Resources.WindowTitleNoFile,
                };
            }
        }

        public bool IsRecentFilesNotEmpty
        {
            get
            {
                return RecentFiles.Count != 0;
            }
        }

        public string CharactersNumber
        {
            get
            {
                return $"{Text.Length} {Properties.Resources.Characters}";
            }
        }

        public string LinesNumber
        {
            get
            {
                return $"{Text.Split('\n').Length} {Properties.Resources.Lines}";
            }
        }

        private void UpdateNewLine()
        {
            if (Text.Contains("\n"))
            {
                if (Text.Contains("\r\n"))
                {
                    NewLine = "\r\n";
                }
                else
                {
                    NewLine = "\n";
                }
            }
            else
            {
                NewLine = "\r\n";
            }
        }

        private string newLine = "\r\n";
        public string NewLine
        {
            get
            {
                return newLine;
            }
            private set
            {
                if (newLine != value)
                {
                    newLine = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateCounters()
        {
            OnPropertyChanged(nameof(CharactersNumber));
            OnPropertyChanged(nameof(LinesNumber));
        }
    }
}