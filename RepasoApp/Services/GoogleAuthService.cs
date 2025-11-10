using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Duende.IdentityModel.Jwk;
using Duende.IdentityModel.OidcClient;
using RepasoApp.Data;
using RepasoApp.Utils;

namespace RepasoApp.Services;

public class GoogleAuthService
{

    public async Task<Usuario> LoginAsync(Usuario user)//llamarle desde el main view model
    {
        //recibir un user para logear (parametro) user -el q recibe. Usuario -el q retornamos
        
        //conexion
        var database = new AppDbContext();
        database.Database.EnsureCreated();//crea la ddbb si no existe
        database.Dispose();//cerrar
        
        var client = await CreateClient();
        
        //preguntar si el user tiene refreshtoken
        if (!string.IsNullOrEmpty(user.RefreshToken)) //Lgante (ni no es null el token ese...)
        {
            var refreshResult = await client.RefreshTokenAsync(user.RefreshToken);//verifica la sesion
            //preguntamos si hay error
            if (!refreshResult.IsError) //si no hay error...
            {
                Console.WriteLine("Sesion restaurada");
                //falta actualizar el refrestoken en la bbdd
                //y nos vamos
                return user;
            }
            Console.WriteLine("No se pudo comprobar la sesion (el token), asi q te toca logear d nuevo");
        }

        //llamar a un metodo asincrono, la llamada debe ser asincrona (await)
        var result = await client.LoginAsync();//abre el navegador y logea, arroja un resultado de eso
        if (result.IsError) //si el resultado es error...
        {
            Console.WriteLine("error al registrar user");
            return null;
        }

        var googlesub = result.User.FindFirst("sub")?.Value; 
        var email = result.User.FindFirst("email")?.Value;
        var imagen = result.User.FindFirst("picture")?.Value;
        var nombre = result.User.FindFirst("name")?.Value;
        Console.WriteLine(googlesub+" "+email+" "+imagen+" "+nombre);

        var db = new AppDbContext();
        var usuario = db.Usuarios.FirstOrDefault(U => U.GoogleSub == googlesub);
        //busca si lo obtenido está en la bbdd
        if (usuario == null)//si es null no existe y hay que registrarlo
        {
            usuario = new Usuario()
            {
                Email = email, GoogleSub = googlesub, Nombre = nombre, ImageUrl = imagen, FechaRegistro = DateTime.Now,
                RefreshToken = result.RefreshToken, UltimoLogin = DateTime.Now
            }; 
            //ahora hay q instancialo e insertarlo en la tabla
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync(); //guardar
            db.Dispose();//cerrar conexion
            Console.WriteLine("Usuario registro exitosamente");
            return usuario;
        }
        else
        {
            Console.WriteLine("Usuario ya existe");
            return usuario;
        }

    }

    public async Task<OidcClient> CreateClient()
    {
        using var http = new HttpClient();
        var keySet = await http.GetStringAsync("https://www.googleapis.com/oauth2/v3/certs");
        var jwks = new JsonWebKeySet(keySet);

        var options = new OidcClientOptions
        {
            Authority = "https://accounts.google.com",
            ClientId = "34324930377-idilvk4l2h103ig8crbu3a170r7t5hi7.apps.googleusercontent.com",
            ClientSecret = "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",
            Scope = "openid profile email",
            RedirectUri = "http://127.0.0.1:7890/",
            Browser = new SystemBrowser(7890),
            ProviderInformation = new ProviderInformation
            {
                AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth",
                TokenEndpoint = "https://oauth2.googleapis.com/token",
                UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo",
                IssuerName = "https://accounts.google.com",
                KeySet = jwks
            }
        };

        return new OidcClient(options);
    }
}