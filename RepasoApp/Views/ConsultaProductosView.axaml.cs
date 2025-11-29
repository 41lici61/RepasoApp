using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RepasoApp.Services;
using RepasoApp.ViewModels;

namespace RepasoApp.Views;

public partial class ConsultaProductosView : UserControl
{
    public ConsultaProductosView()
    {
        InitializeComponent();
        DataContext = new ConsultaProductosViewModel();
    }
    

}