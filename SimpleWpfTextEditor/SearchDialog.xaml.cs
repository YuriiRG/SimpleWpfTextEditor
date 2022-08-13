using SimpleWpfTextEditor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SearchDialog.xaml
    /// </summary>
    public partial class SearchDialog : Window
    {
        private SelectTextDelegate SelectTextFunction;
        private int cursorPosition = 0;
        private ApplicationData Data;
        public SearchDialog(SelectTextDelegate selectText, ApplicationData data)
        {
            SelectTextFunction = selectText;
            Data = data;
            InitializeComponent();
            SearchString.Focus();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FindNext(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchString = SearchString.Text;
                string text = Data.Text;
                if (MatchCaseCheckBox.IsChecked == false)
                {
                    text = text.ToLower();
                    searchString = searchString.ToLower();
                }
                int position = text.IndexOf(searchString, cursorPosition+1);
                if (position == -1)
                {
                    if (!AreOccurrencesExist())
                    {
                        MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                        return;
                    }
                    MessageBox.Show(Properties.Resources.NoOccurrencesFirstShown,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                    cursorPosition = 0;
                    FindNext(null!, null!);
                    return;
                }
                cursorPosition = position;
                SelectTextFunction(position, SearchString.Text.Length);
                SearchString.Focus();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                cursorPosition = 0;
                return;
            }
        }

        private void ResetCursorPosition(object sender, TextChangedEventArgs e)
        {
            cursorPosition = 0;
        }

        private void FindPrevious(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchString = SearchString.Text;
                string text = Data.Text;
                if (MatchCaseCheckBox.IsChecked == false)
                {
                    text = text.ToLower();
                    searchString = searchString.ToLower();
                }
                text = text.Substring(0, cursorPosition);
                int position = text.LastIndexOf(searchString, cursorPosition);
                if (position == -1)
                {
                    if (!AreOccurrencesExist())
                    {
                        MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                        return;
                    }
                    MessageBox.Show(Properties.Resources.NoOccurrencesLastShown,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                    cursorPosition = Data.Text.Length-1;
                    FindPrevious(null!, null!);
                    return;
                }
                cursorPosition = position;
                SelectTextFunction(position, SearchString.Text.Length);
                SearchString.Focus();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                cursorPosition = 0;
                return;
            }
        }
        private bool AreOccurrencesExist()
        {
            if ((bool)MatchCaseCheckBox.IsChecked!)
            {
                return Data.Text.IndexOf(SearchString.Text) != -1;
            }
            else
            {
                return Data.Text.ToLower().IndexOf(SearchString.Text.ToLower()) != -1;
            }
        }

        private void SearchString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchString.IsFocused)
            {
                FindNext(null!, null!);
            }
        }
    }
    public delegate void SelectTextDelegate(int position, int length);
}
