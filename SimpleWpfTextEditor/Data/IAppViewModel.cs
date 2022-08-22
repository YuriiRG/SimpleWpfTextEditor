using System.Collections.ObjectModel;

namespace SimpleWpfTextEditor.Data
{
    public interface IAppViewModel
    {
        public ObservableCollection<string> RecentFiles { get; set; }
        
        public void RecentFilesInsert(int index, string newRecentFile);

        public void RecentFilesRemoveAt(int index);

        public void RecentFilesClear();

        public string FontFamily { get; set; }

        public double FontSize { get; set; }

        public bool WrapText { get; set; }

        public string Locale { get; set; }

        public int GetLocaleIndex();

        public string CurrentFilePath { get; set; }

        public string Text { get; set; }

        public FileStates CurrentFileState { get; }

        public void EventHappened(FileEvents newEvent);

        public string WindowTitle { get; }

        public bool IsRecentFilesNotEmpty { get; }

        public string CharactersNumber { get; }

        public string LinesNumber { get; }

        public string NewLine { get; }

    }
}
