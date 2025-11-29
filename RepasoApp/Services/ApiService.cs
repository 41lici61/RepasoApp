using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Newtonsoft.Json;
using RepasoApp.Models;

namespace RepasoApp.Services
{
    public class ApiService
    {
        private readonly HttpClient client;

        public ApiService()
        {
            client = new HttpClient();

            // Ajusta la IP/BaseAddress y apikey si es necesario:
            client.BaseAddress = new Uri("http://192.160.50.29:7000/");
            client.DefaultRequestHeaders.Add("apikey",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyAgCiAgICAicm9sZSI6ICJhbm9uIiwKICAgICJpc3MiOiAic3VwYWJhc2UtZGVtbyIsCiAgICAiaWF0IjogMTY0MTc2OTIwMCwKICAgICJleHAiOiAxNzk5NTM1NjAwCn0.dc_X5iR_VP_qT0zsiyj_I_OZ2T9FtRU2BBNWN8Bu4GE");
        }

        public async Task<bool> CrearProducto(ProductModel producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));
            var json = JsonConvert.SerializeObject(producto);
            var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/productos")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Prefer", "return=representation");
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<AvaloniaList<ProductModel>> GetProductos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/productos");
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AvaloniaList<ProductModel>>(body);
        }

        public async Task<bool> ModificarProducto(ProductModel producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));
            // Serializamos solo las propiedades que queremos actualizar
            var updateObj = new
            {
                codigo_barras = producto.CodigoBarras,
                descripcion = producto.Descripcion,
                cantidad = producto.Cantidad,
                categoria = producto.Categoria,
                fecha_entrada = producto.FechaEntrada,
                activo = producto.Activo
            };
            var json = JsonConvert.SerializeObject(updateObj);
            var request = new HttpRequestMessage(HttpMethod.Patch, $"rest/v1/productos?id=eq.{producto.Id}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Prefer", "return=representation");
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarProducto(long id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"rest/v1/productos?id=eq.{id}");
            request.Headers.Add("Prefer", "return=representation");
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}

