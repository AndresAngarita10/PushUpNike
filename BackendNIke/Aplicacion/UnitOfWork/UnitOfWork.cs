using Aplicacion.Repository;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.UnitOfWork;
public class UnitOfWork  : IUnitOfWork, IDisposable
{
    private readonly ApiContext _context;

    private RolRepository _Rol;
    private UsuarioRepository _usuarios;

    public UnitOfWork(ApiContext context)
    {
        _context = context;
    }
    
    public IRol Roles
    {
        get{
            if(_Rol== null)
            {
                _Rol= new RolRepository(_context);
            }
            return _Rol;
        }
    }
    
    public IUsuario Usuarios
    {
        get{
            if(_usuarios== null)
            {
                _usuarios= new UsuarioRepository(_context);
            }
            return _usuarios;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
