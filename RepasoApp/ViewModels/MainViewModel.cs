using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Data;
using RepasoApp.Services;

namespace RepasoApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";
    [ObservableProperty] private string imageURL;
    [ObservableProperty] private AvaloniaList<Usuario> listaUsuarios = new(); //del DBService.cs

    [RelayCommand]
    public async Task ObtenerUsuariosAsync()//se pone async cuando el metodo es asincrono
    //no es obligatorio pero es como una norma no escrita.
    //devuelve task aunq no se use xq lo recomienda
    {
        ListaUsuarios = await new DBService().ObtenerUsuarios();
    } //usado en el listbox de mainview

    [RelayCommand]
    public async Task LoginUsuarioAsync(Usuario user) //para login
    {
        var authservice = new GoogleAuthService();
        var usuario = await authservice.LoginAsync(user);
    }

    [RelayCommand]
    public async Task RegisterUserAsync()//poner un boton en el main view
    {
        var authservice = new GoogleAuthService();
        Usuario u = await authservice.LoginAsync(new Usuario());//es un metodo asincrono asi q poner await, lo q obliga a q
        //el metodo tbn sea asincrono (async)
        if (u!=null)
        {
           ImageURL = u.ImageUrl; 
        }
    }
}