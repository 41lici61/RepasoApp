using System;
using Newtonsoft.Json;

namespace RepasoApp.Models;

public class ProductModel
{
    //ponerle los atributos de la tabla de la bbdd del puerto 7000
    //instalar Newtonsoft.Json
    
    //[JsonProperty("id")] public string Id { get; set; } //da error
    [JsonProperty("ref")] public string Ref { get; set; }
    [JsonProperty("color")] public string Color { get; set; }
    [JsonProperty("peso")] public decimal Peso { get; set; }
    [JsonProperty("diametro")] public decimal Diametro { get; set; }
    
    //metodo insertar en el apiservice, se le pasa un producto lo transforma y lo inserta
    
    
    
}