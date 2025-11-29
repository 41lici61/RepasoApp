using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Models;
using RepasoApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DialogHostAvalonia;

namespace RepasoApp.ViewModels
{
    public partial class EditarProductoViewModel : ObservableObject
    {
        private readonly ApiService _api = new ApiService();

        [ObservableProperty] private ProductModel editable;
        [ObservableProperty] private string mensaje;

        public EditarProductoViewModel(ProductModel original)
        {
            Editable = new ProductModel
            {
                Id = original.Id,
                CodigoBarras = original.CodigoBarras,
                Descripcion = original.Descripcion,
                Cantidad = original.Cantidad,
                Categoria = original.Categoria,
                FechaEntrada = original.FechaEntrada,
                Activo = original.Activo
            };
        }

        [RelayCommand]
        public async Task Aceptar()
        {
            Mensaje = "";

            var context = new ValidationContext(Editable);
            var results = new System.Collections.Generic.List<ValidationResult>();

            if (!Validator.TryValidateObject(Editable, context, results, true))
            {
                Mensaje = string.Join("; ", results);
                return;
            }

            var ok = await _api.ModificarProducto(Editable);

            if (ok)
            {
                DialogHost.Close("RootDialog", true);
            }
            else
            {
                Mensaje = "Error actualizando producto";
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            DialogHost.Close("RootDialog", false);
        }
    }
}


