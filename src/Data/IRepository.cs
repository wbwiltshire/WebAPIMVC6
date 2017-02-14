using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeagullConsulting.WebAPIMVC6Website.Data.Interfaces
{
    public enum Action { Add, Update, Delete};

    public interface IPrimaryKey
    {
        object Key { get; set; }
        bool IsIdentity { get; set; }
    }

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        Task<IPager<TEntity>> FindAll(IPager<TEntity> pager);
        Task<ICollection<TEntity>> FindAll();
        Task<TEntity> FindByPK(IPrimaryKey pk);
        Task<int> Add(TEntity entity);
        Task<int> Update(TEntity entity);
        Task<int> Delete(PrimaryKey pk);
        Task<bool> Save();
        Task<bool> Rollback();
    }

    public interface IMapToObject<TEntity>
        where TEntity : class
    {
        TEntity Execute(SqlDataReader reader);
    }
    public interface IMapFromObject<TEntity>
        where TEntity : class
    {
        void Execute(TEntity entity, SqlCommand cmd);
    }

}
