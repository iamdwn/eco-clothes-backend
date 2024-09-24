using System.Linq.Expressions;

namespace DataAccess.Base
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);

        T GetByID(object id);

        void Insert(T entity);

        bool Delete(object id);

        void Delete(T entityToDelete);
        bool Update(object id, T entityToUpdate);
        void Update(T entityToUpdate);
    }
}
