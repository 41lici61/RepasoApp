  

Proyecto de gestión de productos

Hecho por Alicia Ramos Martínez 

### App de gestión de productos en Avalonia (.NET) con:  
- Login con Google  
- Menú lateral (oculto hasta hacer login)  
- Alta, consulta, edición y borrado de productos  
- Datos en Supabase (Docker) + caché local en SQLite  

### Funcionalidades obligatorias  
- Autenticación Google (Duende.IdentityModel.OidcClient)  
- Menú lateral con FluentAvaloniaUI  
- Alta de producto (código barras, descripción, cantidad, categoría, fecha, booleano) + validaciones  
- Lista de productos con:  
  - Eliminar → DialogHost de confirmación  
  - Editar → DialogHost con vista y ViewModel propios  
- Todo con Material.Avalonia + Material.Icons.Avalonia  

### Paquetes NuGet principales  
```
FluentAvaloniaUI
Material.Avalonia
Material.Icons.Avalonia
DialogHost.Avalonia
Xaml.Behaviors.Avalonia
Duende.IdentityModel.OidcClient
Microsoft.EntityFrameworkCore.Sqlite
Newtonsoft.Json
```
