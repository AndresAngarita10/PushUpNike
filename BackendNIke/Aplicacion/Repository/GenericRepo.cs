using System.Linq.Expressions;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
{
    private readonly ApiContext _context;

    public GenericRepo(ApiContext context)
    {
        _context = context;
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }


    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual Task<T> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>()
            .Update(entity);
    }
    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string _search)
    {
        var totalRegistros = await _context.Set<T>().CountAsync();
        var registros = await _context.Set<T>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, int _search)
    {
        var totalRegistros = await _context.Set<T>().CountAsync();
        var registros = await _context.Set<T>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }


    /* public async Task<IEnumerable<object>> ObtenerVentasPorEmpleadoEn2023Async(int Año)
    {
        var ventasPorEmpleado = await (
            from dm in _context.DetalleMovimientos
            join i in _context.InventarioMedicamentos on dm.InventMedicamentoIdFk equals i.Id
            join p in _context.Personas on i.PersonaIdFk equals p.Id
            join d in _context.MovimientoInventarios on dm.MovInventarioIdFk equals d.Id
            where d.TipoMovInventIdFk == 1
            where d.FechaMovimiento.Year == Año
            select new
            {
                Empleado = p.Nombre,
                CantidadVentas = 1,
            }).ToListAsync();

        var ventasPorEmpleadoLista = ventasPorEmpleado
            .GroupBy(x => x.Empleado)
            .Select(g => new
            {
                Empleado = g.Key,
                CantidadVentas = g.Sum(x => x.CantidadVentas),
            })
            .ToList();

        return ventasPorEmpleadoLista;
    } */

}
