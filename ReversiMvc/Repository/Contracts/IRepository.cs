namespace ReversiMvc.Repository.Contracts;

/// <summary>
/// Provides an interface for games repository.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Add(T entity);

    /// <summary>
    /// Returns all entities of the repository.
    /// </summary>
    /// <returns>The entities.</returns>
    IEnumerable<T> All();

    /// <summary>
    /// Updates the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Whether the update was successful or not.</returns>
    bool Update(T entity);

    /// <summary>
    /// Deletes the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Whether the delete was successful or not.</returns>
    bool Delete(T entity);

}
