using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RepasoApp.Models;

public class ProductModel
{
    [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Id { get; set; } // supabase usa id tipo text/uuid; en las peticiones lo usamos como string

    [Required(ErrorMessage = "Código de barras requerido")]
    [JsonProperty("codigo_barras")]
    public string CodigoBarras { get; set; }

    [Required(ErrorMessage = "Descripción requerida")]
    [StringLength(200)]
    [JsonProperty("descripcion")]
    public string Descripcion { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Cantidad debe ser >= 0")]
    [JsonProperty("cantidad")]
    public int Cantidad { get; set; }

    [Required]
    [JsonProperty("categoria")]
    public string Categoria { get; set; }

    [JsonProperty("fecha_entrada")]
    public DateTime FechaEntrada { get; set; } = DateTime.Now;

    [JsonProperty("activo")]
    public bool Activo { get; set; } = true;
}