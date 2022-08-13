using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleWpfTextEditor.Data
{
    public class ApplicationData : INotifyPropertyChanged
    {
        private AppSettings settings = SettingsWriter.Read();

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
                    OnPropertyChanged("IsRecentFilesNotEmpty");
                    SettingsWriter.Save(settings);
                }
            }
        }

        public void RecentFilesInsert(int index, string newRecentFile)
        {
            RecentFiles.Insert(index, newRecentFile);
            OnPropertyChanged("RecentFiles");
            OnPropertyChanged("IsRecentFilesNotEmpty");
            SettingsWriter.Save(settings);
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
            OnPropertyChanged("RecentFiles");
            OnPropertyChanged("IsRecentFilesNotEmpty");
            SettingsWriter.Save(settings);
        }

        public void RecentFilesClear()
        {
            RecentFiles.Clear();
            OnPropertyChanged("RecentFiles");
            OnPropertyChanged("IsRecentFilesNotEmpty");
            SettingsWriter.Save(settings);
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
                    SettingsWriter.Save(settings);
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
                    SettingsWriter.Save(settings);
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
                    SettingsWriter.Save(settings);
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
                    SettingsWriter.Save(settings);
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
                }
            }
        }

        private string windowTitle = "No file opened";
        public string WindowTitle
        {
            get
            {
                return windowTitle;
            }
            set
            {
                if (windowTitle != value)
                {
                    windowTitle = value;
                    OnPropertyChanged();
                }
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

        private string newLine = "\r\n";
        public string NewLine
        {
            get
            {
                return newLine;
            }
            set
            {
                if (newLine != value)
                {
                    newLine = value;
                    OnPropertyChanged();
                }
            }
        }

        public void UpdateCounters()
        {
            OnPropertyChanged("CharactersNumber");
            OnPropertyChanged("LinesNumber");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}