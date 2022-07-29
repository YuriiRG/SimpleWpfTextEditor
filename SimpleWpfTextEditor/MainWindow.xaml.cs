﻿using Microsoft.Win32;
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
        private ApplicationData applicationData = new();
        public MainWindow()
        {
            InitializeComponent();
            Binding textBoxBinding = new()
            {
                Source = applicationData,
                Path = new PropertyPath("Text"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            MainTextBox.SetBinding(TextBox.TextProperty, textBoxBinding);
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Plain text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                applicationData.CurrentFilePath = openFileDialog.FileName;
                applicationData.Text = File.ReadAllText(openFileDialog.FileName);
                this.Title = applicationData.CurrentFilePath;
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(applicationData.CurrentFilePath, applicationData.Text);
            this.Title = applicationData.CurrentFilePath;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.Title.EndsWith('*') && applicationData.CurrentFilePath != String.Empty)
            {
                Title += "*";
            }
        }
    }
}
