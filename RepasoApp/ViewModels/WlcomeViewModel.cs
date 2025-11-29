using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Services;
using RepasoApp.Data;
using RepasoApp.Services;
using System.Threading.Tasks;
using Avalonia.Collections;
using System.Linq;

namespace RepasoApp.ViewModels
{
    public partial class WelcomeViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;

        [ObservableProperty] private Usuario usuario;
        [ObservableProperty] private string message;

        public WelcomeViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _ = LoadUsuario();
        }

        private async Task LoadUsuario()
        {
            var db = new DBService();
            var lista = await db.ObtenerUsuarios();
            Usuario = lista.FirstOrDefault();
        }

        [RelayCommand]
        public void IrAlta()
        {
            _navigationService.Navigate("AltaProducto");
        }

        [RelayCommand]
        public void IrConsulta()
        {
            _navigationService.Navigate("ConsultaProductos");
        }

        [RelayCommand]
        public void Logout()
        {
            _navigationService.Navigate("Login");
        }
    }
}