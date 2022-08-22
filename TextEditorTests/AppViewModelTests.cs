namespace TextEditorTests
{
    [TestClass]
    public class AppViewModelTests
    {
        private AppSettings settings = new();
        private Mock<ISettingsWriter> settingsWriterMock = new();
        public AppViewModelTests()
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
            AppViewModel viewModel = new(settingsWriterMock.Object);
            string fileContent = "This is the content of the file";
            string filePath = "C:\\TestFile.txt";


            viewModel.CurrentFilePath = filePath;
            viewModel.Text = fileContent;
            viewModel.EventHappened(FileEvents.FileOpened);

            
            Assert.AreEqual(viewModel.Text, fileContent);
            Assert.AreEqual(viewModel.CurrentFilePath, filePath);
            Assert.AreEqual(viewModel.WindowTitle, filePath);
            Assert.AreEqual(viewModel.RecentFiles.Count, 1);
            Assert.AreEqual(viewModel.RecentFiles.First(), filePath);
            Assert.AreEqual(viewModel.CurrentFileState, FileStates.FileNoChanges);
            Assert.AreEqual(viewModel.CharactersNumber, $"{fileContent.Length} characters");
            Assert.AreEqual(viewModel.LinesNumber, $"{1} lines");
            Assert.AreEqual(viewModel.NewLine, "\r\n");
            Assert.AreEqual(viewModel.IsRecentFilesNotEmpty, true);
        }

        [TestMethod]
        public void SaveBasicFiles()
        {
            settingsWriterMock.Object.Reset();
            AppViewModel viewModel = new(settingsWriterMock.Object);
            string fileContent = "This is the content of the file";
            string fileEditedContent = "This is the content of the file. Edited";
            string filePath = "C:\\TestFile.txt";


            viewModel.CurrentFilePath = filePath;
            viewModel.Text = fileContent;
            viewModel.EventHappened(FileEvents.FileOpened);
            
            viewModel.Text = fileEditedContent;

            Assert.AreEqual(viewModel.CurrentFileState, FileStates.ChangedFile);
            Assert.AreEqual(viewModel.WindowTitle, $"{filePath}*");

            viewModel.EventHappened(FileEvents.FileSaved);

            Assert.AreEqual(viewModel.Text, fileEditedContent);
            Assert.AreEqual(viewModel.CurrentFilePath, filePath);
            Assert.AreEqual(viewModel.WindowTitle, filePath);
            Assert.AreEqual(viewModel.RecentFiles.Count, 1);
            Assert.AreEqual(viewModel.RecentFiles.First(), filePath);
            Assert.AreEqual(viewModel.CurrentFileState, FileStates.FileNoChanges);
            Assert.AreEqual(viewModel.CharactersNumber, $"{fileEditedContent.Length} characters");
            Assert.AreEqual(viewModel.LinesNumber, $"{1} lines");
            Assert.AreEqual(viewModel.NewLine, "\r\n");
            Assert.AreEqual(viewModel.IsRecentFilesNotEmpty, true);
        }
    }
}
