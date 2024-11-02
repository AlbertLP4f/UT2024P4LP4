using Microsoft.EntityFrameworkCore;
using UT2024P4LP4.Web.Data;
using UT2024P4LP4.Web.Data.Dtos;
using UT2024P4LP4.Web.Data.Entities;

namespace UT2024P4LP4.Web.Services;

public partial class CategoriaService : ICategoriaService
{
    private readonly IApplicationDbContext dbContext;

    public CategoriaService(IApplicationDbContext Context)
    {
        this.dbContext = Context;
    }
    //CRUD
    public async Task<Result> Create(CategoriaRequest categoria)
    {
        try
        {
            var entity = Categoria.Create(categoria.Nombre);
            dbContext.Categorias.Add(entity);
            await dbContext.SaveChangesAsync();
            return Result.Success("✅Producto registrado con exito!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
    public async Task<Result> Update(CategoriaRequest categoria)
    {
        try
        {
            var entity = dbContext.Categorias.Where(c => c.Id == categoria.Id).FirstOrDefault();
            if (entity == null)
                return Result.Failure($"El producto '{categoria.Id}' no existe!");
            if (entity.Update(categoria.Nombre))
            {
                await dbContext.SaveChangesAsync();
                return Result.Success("✅categoria modificado con exito!");
            }
            return Result.Success("🐫 No has realizado ningun cambio!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
    public async Task<Result> Delete(int Id)
    {
        try
        {
            var entity = dbContext.Categorias.Where(p => p.Id == Id).FirstOrDefault();
            if (entity == null)
                return Result.Failure($"El producto '{Id}' no existe!");
            dbContext.Categorias.Remove(entity);
            await dbContext.SaveChangesAsync();
            return Result.Success("✅Producto eliminado con exito!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
    public async Task<ResultList<CategoriaDto>> GetAll(string filtro = "")
    {
        try
        {
            var entities = await dbContext.Categorias
                .Where(c => c.Nombre.ToLower().Contains(filtro.ToLower()))
                .Select(c => new CategoriaDto(c.Id, c.Nombre))
                .ToListAsync();
            return ResultList<CategoriaDto>.Success(entities);
        }
        catch (Exception Ex)
        {
            return ResultList<CategoriaDto>.Failure($"☠️ Error: {Ex.Message}");
        }
    }
}