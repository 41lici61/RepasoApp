using Microsoft.EntityFrameworkCore;

namespace RepasoApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }//maneja la tabla usuarios

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=usuarios.db");
    }//como se va a llamar la bbdd
    //ir a googleouthservice, a donde se extrae la info
}