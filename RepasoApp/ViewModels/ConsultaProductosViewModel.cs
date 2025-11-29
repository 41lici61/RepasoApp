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
        [ObservableProperty] private ProductModel productoSeleccionado;
        [ObservableProperty] private string mensaje;

        public ConsultaProductosViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
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
        public async Task Editar(ProductModel producto)
        {
            if (producto == null) return;
            var vm = new EditarProductoViewModel(producto);
            var dialog = new Views.EditarProductoDialog { DataContext = vm };
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result is bool updated && updated)
            {
                await Refrescar();
            }
        }

        [RelayCommand]
        public async Task Eliminar(ProductModel producto)
        {
            if (producto == null) return;
            var vm = new ConfirmDeleteViewModel(producto);
            var dialog = new Views.ConfirmDeleteDialog { DataContext = vm };
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result is bool ok && ok)
            {
                try
                {
                    bool deleted = await _api.EliminarProducto(producto.Id);
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

