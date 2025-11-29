using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Services;
using RepasoApp.Data;
using System.Threading.Tasks;

namespace RepasoApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;

        [ObservableProperty] private string message;

        public LoginViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public async Task Login()
        {
            Message = "";
            var auth = new GoogleAuthService();
            var user = await auth.LoginAsync(new Usuario());
            if (user != null)
            {
                _navigationService.Navigate("Welcome");
            }
            else
            {
                Message = "Error al iniciar sesión con Google.";
            }
        }
    }
}