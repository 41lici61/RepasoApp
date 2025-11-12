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
    
    //metodo para insertar
    public async Task CrearProducto(ProductModel producto)
    {
        //serializar = convertir a json
            var jsonProduct = JsonConvert.SerializeObject(producto);
            var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/producto_ejemplo") //rest/v1/nombreTabla
            {
                Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")// xq hay que enviar texto plano
            };
            var response = await client.SendAsync(request);//decidir si esta bn o esta mal y retornar boolean
            //ir a mainview model y crear un objeto apiservice
    }
    
    //metodo para modificar
    public async Task<bool> ModificarProducto(ProductModel producto) //usado en main view model
    {
        //serializar = convertir a json
        var jsonProduct = JsonConvert.SerializeObject(producto);
        var request = new HttpRequestMessage(HttpMethod.Patch, "rest/v1/producto_ejemplo?id=eq."+producto.Id) //rest/v1/nombreTabla?id=eq.+id del objeto a modificar
        {
            Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")// xq hay que enviar texto plano
        };
        request.Headers.Add("Prefer", "return=representation"); //cuando se ejecute el response siguinte, devuelva lo que ha actualizado (el objeto)
        var response = await client.SendAsync(request);//decidir si esta bn o esta mal y retornar boolean
        if (!response.IsSuccessStatusCode) //si se ha hecho mal...
        {
            
            throw new Exception("Error al actualizar producto status: " + response.StatusCode);//eleva una excepcion si no funciona la linea de body
            //no hace falta return x el exception
        }
        var body = await response.Content.ReadAsStringAsync();//
        if (String.IsNullOrEmpty(body))
        {
            throw new Exception("Error al actualizar producto status: " + response.StatusCode);
        }

        return true;
    }
    
    //metodo obterner
    public async Task<AvaloniaList<ProductModel>> GetProductos()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/producto_ejemplo");
        //mapear de json a objeto
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<ProductModel>>(listaString);//transforma de string a avalonia list
    }
    
    //metodo para eliminar
    public async Task<bool> EliminarProducto(ProductModel producto) //usado en main view model
    {
        //serializar = convertir a json
        var request = new HttpRequestMessage(HttpMethod.Delete, "rest/v1/producto_ejemplo?id=eq."+producto.Id) //rest/v1/nombreTabla?id=eq.+id del objeto a eliminar
        ;
        request.Headers.Add("Prefer", "return=representation"); //cuando se ejecute el response siguinte, devuelva lo que ha actualizado (el objeto)
        var response = await client.SendAsync(request);//decidir si esta bn o esta mal y retornar boolean
        if (!response.IsSuccessStatusCode) //si se ha hecho mal...
        {
            
            throw new Exception("Error al eliminar producto status: " + response.StatusCode);//eleva una excepcion si no funciona la linea de body
            //no hace falta return x el exception
        }
        var body = await response.Content.ReadAsStringAsync();//
        if (String.IsNullOrEmpty(body))
        {
            throw new Exception("Error al eliminar producto status: " + response.StatusCode);
        }

        return true;
    }


}