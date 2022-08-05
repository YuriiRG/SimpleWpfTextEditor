namespace SimpleWpfTextEditor
{
    // It's finite-state machine, explanation:
    // https://en.wikipedia.org/wiki/Finite-state_machine
    // https://stackoverflow.com/questions/5923767/
    class FileStateFSM
    {
        public FileStates State { get; private set; } = FileStates.NoFile;

        private readonly FileStates[,] fsm = new FileStates[3, 3]
        {
            // FileOpened,             FileChanged             FileSaved  
            {FileStates.FileNoChanges, FileStates.NoFile,      FileStates.NoFile},        // NoFile
            {FileStates.FileNoChanges, FileStates.ChangedFile, FileStates.FileNoChanges}, // FileNoChanges
            {FileStates.FileNoChanges, FileStates.ChangedFile, FileStates.FileNoChanges}  // ChangedFile
        };

        public void EventHappened(FileEvents newEvent)
        {
            State = fsm[(int)State, (int)newEvent];
        }
    }
    enum FileStates
    {
        NoFile,
        FileNoChanges,
        ChangedFile
    }
    enum FileEvents
    {
        FileOpened,
        FileChanged,
        FileSaved
    }
}
