// RepasoApp/MainWindow.axaml.cs
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DialogHost.Avalonia;
using RepasoApp.Services;
using RepasoApp.ViewModels;
using RepasoApp.Views;
using System.Threading.Tasks;

namespace RepasoApp
{
    public partial class MainWindow : Window
    {
        private NavigationService _navigationService = new();
        private ContentControl _contentHolder;
        private Controls.NavigationView _navView;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _contentHolder = this.FindControl<ContentControl>("ContentHolder");
            _navView = this.FindControl<Controls.NavigationView>("NavView");

            // Register routes
            _navigationService.Register("Login", () =>
            {
                _contentHolder.Content = new LoginView { DataContext = new LoginViewModel(_navigationService) };
            });
            _navigationService.Register("Welcome", () =>
            {
                _contentHolder.Content = new WelcomeView { DataContext = new WelcomeViewModel(_navigationService) };
            });
            _navigationService.Register("AltaProducto", () =>
            {
                _contentHolder.Content = new AltaProductoView { DataContext = new AltaProductoViewModel(_navigationService) };
            });
            _navigationService.Register("ConsultaProductos", () =>
            {
                _contentHolder.Content = new ConsultaProductosView { DataContext = new ConsultaProductosViewModel(_navigationService) };
            });

            // Start at Login
            _navigationService.Navigate("Login");

            // Listen to NavigationView menu clicks
            var navView = this.FindControl<FluentAvalonia.UI.Controls.NavigationView>("NavView");
            navView.ItemInvoked += NavView_ItemInvoked;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void NavView_ItemInvoked(object sender, FluentAvalonia.UI.Controls.NavigationViewItemInvokedEventArgs e)
        {
            if (e.InvokedItemContainer is FluentAvalonia.UI.Controls.NavigationViewItem item)
            {
                var tag = item.Tag?.ToString();
                if (tag == "Logout")
                {
                    // on logout go to login
                    _navigationService.Navigate("Login");
                }
                else
                {
                    _navigationService.Navigate(tag);
                }
            }
        }
    }
}

