using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleWpfTextEditor
{
    class ApplicationData : INotifyPropertyChanged
    {
        private string text = String.Empty;
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

        public string CurrentFilePath { get; set; } = String.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
