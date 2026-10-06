using ELearningPlatform.Data;
using ELearningPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories;

/// <summary>Shared CRUD plumbing so the concrete repositories contain only their own queries.</summary>
public abstract class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> Set;

    protected GenericRepository(AppDbContext context)
    {
        Context = context;
        Set = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await Set.FindAsync(id);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync() =>
        await Set.AsNoTracking().ToListAsync();

    public virtual async Task AddAsync(T entity)
    {
        await Set.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        Set.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        Set.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
