// RepasoApp/Services/DBService.cs
using System.Threading.Tasks;
using Avalonia.Collections;
using Microsoft.EntityFrameworkCore;
using RepasoApp.Data;

namespace RepasoApp.Services
{
    public class DBService
    {
        public async Task<AvaloniaList<Usuario>> ObtenerUsuarios()
        {
            await using var db = new AppDbContext();
            await db.Database.EnsureCreatedAsync();
            var Lista = await db.Usuarios.ToListAsync();
            return new AvaloniaList<Usuario>(Lista);
        }
    }
}