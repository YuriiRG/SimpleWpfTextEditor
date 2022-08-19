namespace TextEditorTests
{
    [TestClass]
    public class ApplicationDataTests
    {
        private AppSettings settings = new();
        private Mock<ISettingsWriter> settingsWriterMock = new();
        public ApplicationDataTests()
        {
            Thread.CurrentThread.CurrentUICulture = new("en");

            settingsWriterMock.Setup(x => x.Read()).Returns(settings);
            settingsWriterMock.Setup(x => x.Save(It.IsAny<AppSettings>()))
                .Callback((AppSettings newSettings) =>
                {
                    settings = newSettings;
                });
            settingsWriterMock.Setup(x => x.Reset())
                .Callback(() =>
                {
                    settings = new();
                });
        }

        [TestMethod]
        public void OpenBasicFiles()
        {
            settingsWriterMock.Object.Reset();
            ApplicationData data = new(settingsWriterMock.Object);
            string fileContent = "This is the content of the file";
            string filePath = "C:\\TestFile.txt";


            data.CurrentFilePath = filePath;
            data.Text = fileContent;
            data.EventHappened(FileEvents.FileOpened);

            
            Assert.AreEqual(data.Text, fileContent);
            Assert.AreEqual(data.CurrentFilePath, filePath);
            Assert.AreEqual(data.WindowTitle, filePath);
            Assert.AreEqual(data.RecentFiles.Count, 1);
            Assert.AreEqual(data.RecentFiles.First(), filePath);
            Assert.AreEqual(data.CurrentFileState, FileStates.FileNoChanges);
            Assert.AreEqual(data.CharactersNumber, $"{fileContent.Length} characters");
            Assert.AreEqual(data.LinesNumber, $"{1} lines");
            Assert.AreEqual(data.NewLine, "\r\n");
            Assert.AreEqual(data.IsRecentFilesNotEmpty, true);
        }

        [TestMethod]
        public void SaveBasicFiles()
        {
            settingsWriterMock.Object.Reset();
            ApplicationData data = new(settingsWriterMock.Object);
            string fileContent = "This is the content of the file";
            string fileEditedContent = "This is the content of the file. Edited";
            string filePath = "C:\\TestFile.txt";


            data.CurrentFilePath = filePath;
            data.Text = fileContent;
            data.EventHappened(FileEvents.FileOpened);
            
            data.Text = fileEditedContent;

            Assert.AreEqual(data.CurrentFileState, FileStates.ChangedFile);
            Assert.AreEqual(data.WindowTitle, $"{filePath}*");

            data.EventHappened(FileEvents.FileSaved);

            Assert.AreEqual(data.Text, fileEditedContent);
            Assert.AreEqual(data.CurrentFilePath, filePath);
            Assert.AreEqual(data.WindowTitle, filePath);
            Assert.AreEqual(data.RecentFiles.Count, 1);
            Assert.AreEqual(data.RecentFiles.First(), filePath);
            Assert.AreEqual(data.CurrentFileState, FileStates.FileNoChanges);
            Assert.AreEqual(data.CharactersNumber, $"{fileEditedContent.Length} characters");
            Assert.AreEqual(data.LinesNumber, $"{1} lines");
            Assert.AreEqual(data.NewLine, "\r\n");
            Assert.AreEqual(data.IsRecentFilesNotEmpty, true);
        }
    }
}
