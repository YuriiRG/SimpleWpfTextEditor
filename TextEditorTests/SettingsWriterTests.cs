using System.Collections.ObjectModel;

namespace TextEditorTests
{
    [TestClass]
    public class SettingsWriterTests
    {
        [TestMethod]
        public void WriteReadSettings()
        {
            AppSettings sampleSettings = new()
            {
                RecentFiles =
                {
                    "D:\\mytemp\\visualstudio\\newlinetest\\LF.txt",
                    "D:\\mytemp\\visualstudio\\test4.txt",
                    "D:\\mytemp\\visualstudio\\newlinetest\\CRLF.txt"
                },
                FontFamily = "Wingdings",
                FontSize = 18,
                WrapText = false,
                Locale = "ru-RU"
            };

            SettingsWriter.Save(sampleSettings);
            var actualSettings = SettingsWriter.Read();

            Assert.IsTrue(Enumerable.SequenceEqual(sampleSettings.RecentFiles, actualSettings.RecentFiles));
            Assert.AreEqual(sampleSettings.FontFamily, actualSettings.FontFamily);
            Assert.AreEqual(sampleSettings.FontSize, actualSettings.FontSize);
            Assert.AreEqual(sampleSettings.WrapText, actualSettings.WrapText);
            Assert.AreEqual(sampleSettings.Locale, actualSettings.Locale);

        }
    }
}
