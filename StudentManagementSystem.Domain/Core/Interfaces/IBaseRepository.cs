using System.Linq.Expressions;

namespace StudentManagementSystem.Domain.Core.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{

    virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    virtual Task<TEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    virtual Task<ICollection<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken ct = default)
        => throw new NotImplementedException();

    virtual Task<PaginatedResult<TEntity>> GetPaginatedAsync(
        int pageNumber,
        int PageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? inculde = null,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}

public record PaginatedResult<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);
