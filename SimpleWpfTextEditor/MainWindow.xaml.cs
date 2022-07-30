using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleWpfTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationData Data;
        public MainWindow()
        {
            InitializeComponent();
            Data = (ApplicationData)DataContext;
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Data.CurrentFilePath = openFileDialog.FileName;
                Data.Text = File.ReadAllText(openFileDialog.FileName);
                Data.WindowTitle = Data.CurrentFilePath;
            }
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e)
        {
            File.WriteAllText(Data.CurrentFilePath, Data.Text);
            Data.WindowTitle = Data.CurrentFilePath;
        }

        private void SaveFileAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                Data.CurrentFilePath = saveFileDialog.FileName;
                File.WriteAllText(Data.CurrentFilePath, Data.Text);
                Data.WindowTitle = Data.CurrentFilePath;
            }
        }

        private void IsFileOpened(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Data.CurrentFilePath != String.Empty);
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Data.WindowTitle.EndsWith('*') &&
                Data.CurrentFilePath != String.Empty)
            {
                Data.WindowTitle += "*";
            }
        }
    }
}