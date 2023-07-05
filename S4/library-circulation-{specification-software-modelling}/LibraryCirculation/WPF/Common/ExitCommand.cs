using System.Windows;

namespace LibraryCirculation.WPF.Common
{
    public class ExitCommand : RelayCommand
    {
        public ExitCommand(Window window)
            : base(_ => { window.Close(); })
        {
        }
    }
}