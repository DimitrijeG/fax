using System.Windows;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI;

public class LogOutCommand : CommandBase
{
    private readonly Window _window;

    public LogOutCommand(Window window)
    {
        _window = window;
    }

    public override void Execute(object parameter)
    {
        _window.Close();
    }
}