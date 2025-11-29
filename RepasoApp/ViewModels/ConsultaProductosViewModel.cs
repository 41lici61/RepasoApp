using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Models;
using RepasoApp.Services;
using Avalonia.Collections;
using System.Threading.Tasks;
using DialogHostAvalonia;
using System.Linq;
using System;

namespace RepasoApp.ViewModels
{
    public partial class ConsultaProductosViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _api = new ApiService();

        [ObservableProperty] private AvaloniaList<ProductModel> listaProductos = new();
        [ObservableProperty] private ProductModel productoSeleccionado = null!;
        [ObservableProperty] private string mensaje = null!;

        

        public ConsultaProductosViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _ = Refrescar();
        }
        
        public ConsultaProductosViewModel()
        {
            _navigationService = new NavigationService();
            _ = Refrescar();
        }

        [RelayCommand]
        public async Task Refrescar()
        {
            Mensaje = "";
            try
            {
                ListaProductos = await _api.GetProductos();
            }
            catch (Exception ex)
            {
                Mensaje = "Error al obtener productos: " + ex.Message;
            }
        }

        [RelayCommand]
        public async Task Editar()
        {
            if (ProductoSeleccionado == null) return;
            var vm = new EditarProductoViewModel(ProductoSeleccionado);
            var dialog = new Views.EditarProductoDialog { DataContext = vm };
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result is bool updated && updated)
            {
                await Refrescar();
            }
        }

        [RelayCommand]
        public async Task Eliminar()
        {
            if (ProductoSeleccionado == null) return;
            var vm = new ConfirmDeleteViewModel(ProductoSeleccionado);
            var dialog = new Views.ConfirmDeleteDialog { DataContext = vm };
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result is bool ok && ok)
            {
                try
                {
                    bool deleted = await _api.EliminarProducto(ProductoSeleccionado.Id);
                    if (deleted)
                    {
                        Mensaje = "Producto eliminado";
                        await Refrescar();
                    }
                    else Mensaje = "No se pudo eliminar el producto";
                }
                catch (Exception ex)
                {
                    Mensaje = "Error eliminando: " + ex.Message;
                }
            }
        }
    }
}

