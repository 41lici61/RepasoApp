// RepasoApp/ViewModels/AltaProductoViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Models;
using RepasoApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepasoApp.ViewModels
{
    public partial class AltaProductoViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _api = new ApiService();

        [ObservableProperty] private ProductModel producto = new();
        [ObservableProperty] private ObservableCollection<string> categorias = new ObservableCollection<string> { "Electrónica", "Alimentos", "Hogar", "General" };
        [ObservableProperty] private string mensaje;

        public AltaProductoViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            
            Producto.Categoria = Categorias.FirstOrDefault();
        }

        [RelayCommand]
        public async Task Guardar()
        {
            Mensaje = "";
            
            var context = new ValidationContext(Producto);
            var results = new System.Collections.Generic.List<ValidationResult>();
            if (!Validator.TryValidateObject(Producto, context, results, true))
            {
                Mensaje = string.Join("; ", results.Select(r => r.ErrorMessage));
                return;
            }

            
            if (string.IsNullOrWhiteSpace(Producto.CodigoBarras))
            {
                Mensaje = "Código de barras inválido";
                return;
            }

            // llamada al api
            bool ok = await _api.CrearProducto(Producto);
            if (ok)
            {
                Mensaje = "Producto creado correctamente";
                
                _navigationService.Navigate("ConsultaProductos");
            }
            else
            {
                Mensaje = "Error al crear producto en el servidor";
            }
        }

        [RelayCommand]
        public void Limpiar()
        {
            Producto = new ProductModel { Categoria = Categorias.FirstOrDefault() };
            Mensaje = "";
        }
    }
}
