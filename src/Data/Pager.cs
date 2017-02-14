using System;
using System.Collections.Generic;

namespace SeagullConsulting.WebAPIMVC6Website.Data
{
    public interface IPager<TEntity> 
        where TEntity : class
    {
        int PageNbr { get; set; }
        int PageSize {get; set; }
        int RowCount { get; set; }
        ICollection<TEntity> Entities { get; set; }
    }
    public class Pager<TEntity> : IPager<TEntity>
        where TEntity : class
    {
        public int PageNbr { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public ICollection<TEntity> Entities { get; set; }
    }
}
