using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using RepasoApp.Services;
using RepasoApp.ViewModels;
using RepasoApp.Views;

namespace RepasoApp
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService = new();
        private ContentControl _contentHolder;
        private NavigationView _navView;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _contentHolder = this.FindControl<ContentControl>("ContentHolder");
            _navView = this.FindControl<NavigationView>("NavView");

            RegisterRoutes();
            HookEvents();

            _navigationService.Navigate("Login");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RegisterRoutes()
        {
            _navigationService.Register("Login", () =>
            {
                _contentHolder.Content = new LoginView
                {
                    DataContext = new LoginViewModel(_navigationService)
                };
            });

            _navigationService.Register("Welcome", () =>
            {
                _contentHolder.Content = new WelcomeView
                {
                    DataContext = new WelcomeViewModel(_navigationService)
                };
            });

            _navigationService.Register("AltaProducto", () =>
            {
                _contentHolder.Content = new AltaProductoView
                {
                    DataContext = new AltaProductoViewModel(_navigationService)
                };
            });

            _navigationService.Register("ConsultaProductos", () =>
            {
                _contentHolder.Content = new ConsultaProductosView
                {
                    DataContext = new ConsultaProductosViewModel(_navigationService)
                };
            });
        }

        private void HookEvents()
        {
            _navView.ItemInvoked += NavView_ItemInvoked;
        }

        private void NavView_ItemInvoked(object sender, NavigationViewItemInvokedEventArgs e)
        {
            if (e.InvokedItemContainer is NavigationViewItem item)
            {
                var tag = item.Tag?.ToString();

                if (tag == "Logout")
                {
                    // VOLVER AL LOGIN
                    _navigationService.Navigate("Login");
                    return;
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    _navigationService.Navigate(tag);
                }
            }
        }
    }
}


