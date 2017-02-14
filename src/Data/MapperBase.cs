using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime;
using Microsoft.Extensions.Logging;
using SeagullConsulting.WebAPIMVC6Website.Data.Interfaces;


namespace SeagullConsulting.WebAPIMVC6Website.Data
{
    public class PrimaryKey : IPrimaryKey
    {
        public object Key { get; set; }
        public bool IsIdentity { get; set; }
    }

    public abstract class MapToObjectBase<TEntity> : IMapToObject<TEntity>
        where TEntity : class
    {
        //protected static readonly ILog log = LogManager.GetLogger(typeof(MapFromObjectBase<TEntity>));
        protected static ILogger logger;
        protected string SQLStatus;

        public abstract TEntity Execute(SqlDataReader reader);
    }
    public abstract class MapFromObjectBase<TEntity> : IMapFromObject<TEntity>
        where TEntity : class
    {
        //protected static readonly ILog log = LogManager.GetLogger(typeof(MapToObjectBase<TEntity>));
        protected static ILogger logger;
        protected string SQLStatus;

        public abstract void Execute(TEntity entity, SqlCommand cmd);
    }

}
