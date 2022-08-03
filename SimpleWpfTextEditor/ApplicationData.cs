using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;

namespace SimpleWpfTextEditor
{
    public class ApplicationData : INotifyPropertyChanged
    {

        // Put JsonIgnore attribute on public properties that should not be saved in settings.json
        private string text = String.Empty;
        [JsonIgnore]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        private string windowTitle = "No file opened";
        [JsonIgnore]
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

        [JsonIgnore]
        public string CurrentFilePath { get; set; } = String.Empty;

        private bool wrapText = true;
        public bool WrapText
        {
            get
            {
                return wrapText;
            }
            set
            {
                if (value != wrapText)
                {
                    wrapText = value;
                    OnPropertyChanged();
                    SettingsWriter.Save(this);
                }
            }
        }

        private double fontSize = 14;
        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (value != fontSize)
                {
                    fontSize = value;
                    OnPropertyChanged();
                    SettingsWriter.Save(this);
                }
            }
        }

        private string fontFamily = "Segoe UI";
        public string FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                if (value != fontFamily)
                {
                    fontFamily = value;
                    OnPropertyChanged();
                    SettingsWriter.Save(this);
                }
            }
        }

        private bool isRecentFilesEmpty = true;
        [JsonIgnore]
        public bool IsRecentFilesNotEmpty
        {
            get
            {
                return isRecentFilesEmpty;
            }
            set
            {
                if (value != isRecentFilesEmpty)
                {
                    isRecentFilesEmpty = value;
                    OnPropertyChanged();
                    SettingsWriter.Save(this);
                }
            }
        }

        public ObservableCollection<string> RecentFiles { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}