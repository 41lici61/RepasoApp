using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Models;
using DialogHostAvalonia;

namespace RepasoApp.ViewModels
{
    public partial class ConfirmDeleteViewModel : ObservableObject
    {
        [ObservableProperty] private string textoConfirmacion;
        private readonly ProductModel _producto;

        public ConfirmDeleteViewModel(ProductModel producto)
        {
            _producto = producto;
            TextoConfirmacion = $"¿Deseas eliminar el producto '{producto.Descripcion}' (Código: {producto.CodigoBarras})?";
        }

        [RelayCommand]
        public void Aceptar()
        {
            
            DialogHost.Close("RootDialog", true);
        }

        [RelayCommand]
        public void Cancelar()
        {
            
            DialogHost.Close("RootDialog", false);
        }
    }
}