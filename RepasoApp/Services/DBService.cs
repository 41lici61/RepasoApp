using System.Threading.Tasks;
using Avalonia.Collections;
using Microsoft.EntityFrameworkCore;
using RepasoApp.Data;

namespace RepasoApp.Services;

public class DBService
{
    public async Task<AvaloniaList<Usuario>> ObtenerUsuarios()
    //Método asincrono que retorna una lista de usuarios, pero retorna un task xq las 
    //operaciones dentro del metodo se ejecutan de forma asincrona
    //o sea tiene que venir envuelto en task x narices 
    {
        // el using genera un contexto d la bbdd y asegura la liberacion de recursos tras
        //terminar en la cara de dani(q se cierre, vaya)
        await using var db = new AppDbContext();
        
        //asegurarse de tener la bbdd creada, si no existe la crea y no bloquea
        //el hilo principal (q no bloquea el flujo de la app (no se pilla)).
        //await indica q hasta q no termine, no sigue.
        await db.Database.EnsureCreatedAsync();
        
        //lista de todos los users, equivalente a select * from usuarios
        //pero mapeado ya a una lista
        var Lista = await db.Usuarios.ToListAsync();
        
        //el retorno tiene q ser un avaloniaList , asi que convertimos la lista
        //a un Avalonia list así ↓ ↓ ↓
        return new AvaloniaList<Usuario>(Lista);
        
        //ahora ir a la vista mainview : es el listbox
    }
}