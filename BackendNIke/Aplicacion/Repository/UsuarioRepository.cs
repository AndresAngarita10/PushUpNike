using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class UsuarioRepository : GenericRepo<Usuario>, IUsuario
{
    private readonly ApiContext _context;

    public UsuarioRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Usuario> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Usuarios
            .Include(u => u.Rols)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
    }

    public async Task<Usuario> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios
            .Include(u => u.Rols)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Nombre.ToLower() == username.ToLower());
    }
    public override async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .ToListAsync();
    }

    public override async Task<Usuario> GetByIdAsync(int id)
    {
        return await _context.Usuarios
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
    public override async Task<(int totalRegistros, IEnumerable<Usuario> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Usuarios as IQueryable<Usuario>;

        if(!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Nombre.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.Id);

        var totalRegistros = await query.CountAsync();
        var registros = await query 
            .Skip((pageIndez - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    /* public async Task<IEnumerable<object>> PropietarioYMascota()
    {
        return await (
            from p in _context.Partners
            join pt in _context.PartnerTypes on p.PartnerTypeIdFk equals pt.Id
            join es in _context.Specialities on p.SpecialtyIdFk equals es.Id
            where pt.Name.Contains("Cliente")
            where es.Name.Contains("Cliente")
            select new
            {
                Name = p.Name,
                Pets = (
                    from pet in _context.Pets
                    join esp in _context.Species on pet.SpeciesIdFk equals esp.Id
                    where pet.UserOwnerId == p.Id
                    select new
                    {
                        Name = pet.Name,
                        Birth = pet.DateBirth,
                        Especies = esp.Name
                    }
                ).ToList()
            }
        ).ToListAsync();
    } */

    
   /*  public async Task<IEnumerable<object>> TotalProvSuministraMedicamentosxAnio(int year)
    {
        DateOnly fechaInicio = DateOnly.FromDateTime(new DateTime(year, 1, 1));
        DateOnly fechaFin = DateOnly.FromDateTime(new DateTime(year, 12, 31));

        var query = from per in _context.Personas
                    join t in _context.Rols on per.RolIdFk equals t.Id
                    where t.Nombre == "Proveedor"
                    where (
                        from p in _context.MovimientoInventarios
                        where p.TipoMovInventIdFk == 2
                        where p.FechaMovimiento >= fechaInicio && p.FechaMovimiento <= fechaFin
                        select p.ResponsableIdFk
                    ).Contains(per.Id)
                    select new {
                        Proveedor = per.Nombre,
                        documento = per.NumeroDocumento
                    };

        return await query.ToListAsync();
    } */
}