namespace Dominio.Interfaces;
public interface IUnitOfWork
{
    IRol Roles { get; }
    IUsuario Usuarios { get; }
    

    Task<int> SaveAsync();
}
