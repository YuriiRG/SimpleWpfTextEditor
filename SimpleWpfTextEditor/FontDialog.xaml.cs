using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            FontSizeTextBox.Text = Convert.ToString(Data.FontSize);
        }

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.FontSize = Convert.ToDouble(FontSizeTextBox.Text);
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

        }
    }
}
