using System;
using System.Windows;
using System.Windows.Media;

namespace SimpleWpfTextEditor
{
    /// <summary>
    /// Логика взаимодействия для FontDialog.xaml
    /// </summary>
    public partial class FontDialog : Window
    {
        private ApplicationData Data;
        public FontDialog(ApplicationData data)
        {
            Data = data;
            InitializeComponent();
            FillFontFamilyComboBox();
            InitializeData();
        }

        private void InitializeData()
        {
            FontSizeTextBox.Text = Convert.ToString(Data.FontSize);
        }

        private void FillFontFamilyComboBox()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                FontFamilyComboBox.Items.Add(fontFamily.Source);
            }

            FontFamilyComboBox.SelectedItem = Data.FontFamily;
        }

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.FontSize = Convert.ToDouble(FontSizeTextBox.Text);
                Data.FontFamily = (string)FontFamilyComboBox.SelectedItem;
            }
            catch
            {
                MessageBox.Show("Invalid data entered",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
