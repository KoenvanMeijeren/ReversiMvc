namespace ReversiMvc.Repository;

public abstract class RepositoryDatabaseBase<T> : IRepository<T> where T : class, IEntity
{
    protected readonly DbContext Context;

    protected readonly DbSet<T> DbSet;

    protected RepositoryDatabaseBase(DbContext context, DbSet<T> dbSet)
    {
        this.Context = context;
        this.DbSet = dbSet;
    }

    /// <inheritdoc />
    public virtual void Add(T entity)
    {
        this.Context.Add(entity);
        this.Context.SaveChanges();
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(T entity)
    {
        this.Context.Add(entity);

        return await this.Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual IEnumerable<T> All()
    {
        return this.DbSet.ToList();
    }

    /// <inheritdoc />
    public Task<List<T>> AllAsync()
    {
        return this.DbSet.ToListAsync();
    }

    /// <inheritdoc />
    public virtual bool Update(T entity)
    {
        this.DbSet.Update(entity);

        return this.Context.SaveChanges() > 0;
    }

    /// <inheritdoc />
    public async Task<int> UpdateAsync(T entity)
    {
        this.DbSet.Update(entity);

        return await this.Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual bool Delete(T entity)
    {
        this.DbSet.Remove(entity);

        return this.Context.SaveChanges() > 0;
    }

    /// <inheritdoc />
    public async Task<int> DeleteAsync(T entity)
    {
        this.DbSet.Remove(entity);

        return await this.Context.SaveChangesAsync();
    }
}
