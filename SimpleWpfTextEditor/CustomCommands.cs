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
        public static RoutedCommand ChangeFont { get; set; } =
            new RoutedCommand("ChangeFont", typeof(MainWindow));
    }
}
