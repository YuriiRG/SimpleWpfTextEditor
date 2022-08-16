using SimpleWpfTextEditor.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleWpfTextEditor
{
    /// <summary>
    /// Interaction logic for SearchDialog.xaml
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
            Find(SearchDirection.Forward);
            
        }

        private void FindPrevious(object sender, RoutedEventArgs e)
        {
            Find(SearchDirection.Backward);
        }

        private void Find(SearchDirection direction)
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
                
                int position = -1;
                
                if (direction == SearchDirection.Forward)
                {
                    position = text.IndexOf(searchString, cursorPosition + 1);
                }
                else
                {
                    text = text.Substring(0, cursorPosition);
                    position = text.LastIndexOf(searchString, cursorPosition);
                }
                
                if (position == -1)
                {
                    if (!AreOccurrencesExist())
                    {
                        MessageBox.Show(Properties.Resources.NoOccurrences,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                        return;
                    }
                    
                    string message = (direction == SearchDirection.Forward) ?
                        Properties.Resources.NoOccurrencesFirstShown :
                        Properties.Resources.NoOccurrencesLastShown;
                    
                    MessageBox.Show(message,
                                    Properties.Resources.Notification,
                                    MessageBoxButton.OK);
                    cursorPosition = (direction == SearchDirection.Forward) ? 0 : (Data.Text.Length - 1);
                    if (direction == SearchDirection.Forward)
                    {
                        Find(SearchDirection.Forward);
                    }
                    else
                    {
                        Find(SearchDirection.Backward);
                    }
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
    enum SearchDirection
    {
        Forward,
        Backward
    }
}
