using StudentManagementSystem.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using StudentManagementSystem.Infrastructre.Data;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Infrastructre.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext context)
    : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(Id, cancellationToken).AsTask();
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (include != null)
            query = include(query);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
    public virtual async Task<PaginatedResult<TEntity>> GetPaginatedAsync(
        int pageNumber,
        int PageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? inculde = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        //applay include
        if (inculde != null)
            query = inculde(query);

        if (predicate != null)
            query = query.Where(predicate);

        int totalCount = await query.CountAsync(cancellationToken);

        if (orderBy != null)
            query = orderBy(query);

        var items = await query
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .AsNoTracking().ToListAsync(cancellationToken);

        return new PaginatedResult<TEntity>(items, totalCount, pageNumber, PageSize);
    }
}

