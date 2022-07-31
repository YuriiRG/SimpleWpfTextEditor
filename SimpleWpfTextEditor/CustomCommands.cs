using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleWpfTextEditor
{
    class CustomCommands
    {
        public static RoutedCommand Preferences { get; set; } =
            new RoutedCommand("Preferences", typeof(MainWindow));
    }
}
