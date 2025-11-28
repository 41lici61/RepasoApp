using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Newtonsoft.Json;
using RepasoApp.Models;

namespace RepasoApp.Services;

public class ApiService
{
    private HttpClient client;
    
    public ApiService()
    {
        client = new HttpClient();
        
        //ir a portainer, contenedor supabase kong - editar - poner puertos 7000 - 8000 y 7443 - 8443, deploy
        //kong - editar - bajar hacia Env - supabase anon key, copiar su valor. el lo que se pone como valor a "apikey"
        
        //indicar dónde está el servidor
        client.BaseAddress = new Uri("http://192.160.50.29:7000/");
        client.DefaultRequestHeaders.Add("apikey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyAgCiAgICAicm9sZSI6ICJhbm9uIiwKICAgICJpc3MiOiAic3VwYWJhc2UtZGVtbyIsCiAgICAiaWF0IjogMTY0MTc2OTIwMCwKICAgICJleHAiOiAxNzk5NTM1NjAwCn0.dc_X5iR_VP_qT0zsiyj_I_OZ2T9FtRU2BBNWN8Bu4GE");
        //esto permite realizar peticiones a la api
        //crear un modelo (en Models)
    }
    
    // En ApiService.cs (revisa BaseAddress & apikey ya puestos)
public async Task<bool> CrearProducto(ProductModel producto)
{
    // Validación previa simple
    if (producto == null) throw new ArgumentNullException(nameof(producto));
    var jsonProduct = JsonConvert.SerializeObject(producto);
    var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/productos")
    {
        Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")
    };
    request.Headers.Add("Prefer","return=representation");
    var response = await client.SendAsync(request);
    if (!response.IsSuccessStatusCode)
        throw new Exception($"Error creando producto: {response.StatusCode}");
    return true;
}

// GetProductos -> lee y retorna lista
public async Task<Avalonia.Collections.AvaloniaList<ProductModel>> GetProductos()
{
    var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/productos");
    var response = await client.SendAsync(request);
    var listaString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<Avalonia.Collections.AvaloniaList<ProductModel>>(listaString);
}

// ModificarProducto: PATCH por id
public async Task<bool> ModificarProducto(ProductModel producto)
{
    if (producto == null) throw new ArgumentNullException(nameof(producto));
    var jsonProduct = JsonConvert.SerializeObject(new {
        codigo_barras = producto.CodigoBarras,
        descripcion = producto.Descripcion,
        cantidad = producto.Cantidad,
        categoria = producto.Categoria,
        fecha_entrada = producto.FechaEntrada,
        activo = producto.Activo
    });
    var request = new HttpRequestMessage(HttpMethod.Patch, $"rest/v1/productos?id=eq.{producto.Id}")
    {
        Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")
    };
    request.Headers.Add("Prefer","return=representation");
    var response = await client.SendAsync(request);
    if (!response.IsSuccessStatusCode) throw new Exception("Error al actualizar producto");
    return true;
}

public async Task<bool> EliminarProducto(string id)
{
    if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
    var request = new HttpRequestMessage(HttpMethod.Delete, $"rest/v1/productos?id=eq.{id}");
    request.Headers.Add("Prefer","return=representation");
    var response = await client.SendAsync(request);
    if (!response.IsSuccessStatusCode) throw new Exception("Error al eliminar producto");
    return true;
}

    
    
    
    


}