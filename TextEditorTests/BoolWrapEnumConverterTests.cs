using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TextEditorTests
{
    [TestClass]
    public class BoolWrapEnumConverterTests
    {
        [TestMethod]
        public void IsConverter()
        {
            IValueConverter converter = new BoolWrapEnumConverter();
            Assert.IsNotNull(converter);
        }

        [TestMethod]
        public void Convert()
        {
            IValueConverter converter = new BoolWrapEnumConverter();

            Assert.AreEqual(TextWrapping.Wrap, converter.Convert(true,
                                                                 typeof(TextWrapping),
                                                                 new object(),
                                                                 new CultureInfo("en")));
            Assert.AreEqual(TextWrapping.Wrap, converter.Convert(true,
                                                                 typeof(TextWrapping),
                                                                 new object(),
                                                                 new CultureInfo("ru-RU")));

            Assert.AreEqual(TextWrapping.NoWrap, converter.Convert(false,
                                                                   typeof(TextWrapping),
                                                                   new object(),
                                                                   new CultureInfo("en")));
            Assert.AreEqual(TextWrapping.NoWrap, converter.Convert(false,
                                                                   typeof(TextWrapping),
                                                                   new object(),
                                                                   new CultureInfo("ru-RU")));
        }

        [TestMethod]
        public void ConvertBack()
        {
            IValueConverter converter = new BoolWrapEnumConverter();

            Assert.AreEqual(true, converter.ConvertBack(TextWrapping.Wrap,
                                                        typeof(bool),
                                                        new object(),
                                                        new CultureInfo("en")));
            Assert.AreEqual(true, converter.ConvertBack(TextWrapping.Wrap,
                                                        typeof(bool),
                                                        new object(),
                                                        new CultureInfo("ru-RU")));

            Assert.AreEqual(false, converter.ConvertBack(TextWrapping.NoWrap,
                                                         typeof(bool),
                                                         new object(),
                                                         new CultureInfo("en")));

            Assert.AreEqual(false, converter.ConvertBack(TextWrapping.NoWrap,
                                                         typeof(bool),
                                                         new object(),
                                                         new CultureInfo("ru-RU")));
        }
    }
}