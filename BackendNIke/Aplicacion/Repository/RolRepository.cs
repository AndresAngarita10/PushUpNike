using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class RolRepository : GenericRepo<Rol>, IRol
{
    private readonly ApiContext _context;

    public RolRepository(ApiContext context) : base(context)
    {
        _context = context;
    }
    public override async Task<IEnumerable<Rol>> GetAllAsync()
    {
        return await _context.Roles
            .ToListAsync();
    }

    public override async Task<Rol> GetByIdAsync(int id)
    {
        return await _context.Roles
        .FirstOrDefaultAsync(p => p.Id == id);
    }
    public override async Task<(int totalRegistros, IEnumerable<Rol> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Roles as IQueryable<Rol>;

        if (!string.IsNullOrEmpty(search))
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
    /*
        public async Task<object> Consulta2B()
        {

            var Movimiento = await (
                from d in _context.DetalleMovimientos
                join m in _context.MovimientoMedicamentos on d.IdMovimientoMedicamentoFk equals m.Id

                select new{
                    idMovimientoMedicamento = m.Id,
                    TipoMovimiento = m.TipoMovimiento.Descripcion,
                    total = d.Precio * d.Cantidad,
                }).Distinct()
                .ToListAsync();

            return Movimiento;
        }
        public virtual async Task<(int totalRegistros,object registros)> Consulta2B(int pageIndez, int pageSize, string search)
        {
            var query = 
                (
                from d in _context.DetalleMovimientos
                join m in _context.MovimientoMedicamentos on d.IdMovimientoMedicamentoFk equals m.Id

                select new{
                    idMovimientoMedicamento = m.Id,
                    TipoMovimiento = m.TipoMovimiento.Descripcion,
                    total = d.Precio * d.Cantidad,
                }).Distinct();

           if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.idMovimientoMedicamento.ToString().ToLower().Contains(search));
            }

            query = query.OrderBy(p => p.idMovimientoMedicamento);
            var totalRegistros = await query.CountAsync();
            var registros = await query 
                .Skip((pageIndez - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (totalRegistros, registros);
        } 
     */

    /* public async Task<IEnumerable<object>> TotalVentasxProveedor()
    {
        
        var query = from p in _context.MovimientoInventarios
                    join e in _context.DetalleMovimientos on p.Id equals e.MovInventarioIdFk
                    join per in _context.Personas on p.ResponsableIdFk equals per.Id
                    join t in _context.Rols on per.RolIdFk equals t.Id
                    where p.TipoMovInventIdFk == 2
                    group new {e, per} by per into g
                    select new
                    {
                        Proveedor = g.Key.Nombre,
                        CantidadProductos = g.Sum(x => x.e.Cantidad),
                        Documento = g.Key.NumeroDocumento 
                    };

        return await query.ToListAsync();
    } */
}