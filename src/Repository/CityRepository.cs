/******************************************************************************************************
 *This class was generated on 04/30/2014 09:06:10 using Repository Builder version 0.9. *
 *The class was generated from Database: Customer and Table: City.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeagullConsulting.WebAPIMVC6Website.Data;
using SeagullConsulting.WebAPIMVC6Website.Data.Interfaces;
using SeagullConsulting.WebAPIMVC6Website.Data.POCO;

namespace SeagullConsulting.WebAPIMVC6Website.Data.Repository
{

    public class CityRepository : RepositoryBase<City>, IRepository<City>
    {
        private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM City WHERE Active=1";
        private const string FINDALL_STMT = "SELECT Id,Name,StateId,Active,ModifiedDt,CreateDt FROM City WHERE Active=1";
        private const string FINDALLPAGER_STMT = "SELECT Id,Name,StateId,Active,ModifiedDt,CreateDt FROM City WHERE Active=1 ORDER BY Id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY";
        private const string FINDBYPK_STMT = "SELECT Id, Name, StateId, Active, ModifiedDt, CreateDt FROM City WHERE Id =@pk";
        private const string ADD_STMT = "INSERT INTO City (Name, StateId, Active, ModifiedDt, CreateDt) VALUES (@p1, @p2, 1, GETDATE(), GETDATE()); SELECT CAST(scope_identity() AS int)";
        private const string UPDATE_STMT = "UPDATE City SET Name=@p1, StateId=@p2, Active=1, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string DELETE_STMT = "UPDATE City SET Active=0, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string ORDERBY_STMT = " ORDER BY ";

        #region ctor
        //Default constructor calls the base ctor
        public CityRepository(ILogger l, IConfigurationRoot c) :
            base(l, c)
        { Init(); }
        public CityRepository(UnitOfWork uow) :
            base(uow)
        { Init(); }

        private void Init()
        {
            //Mapper = new CityMapper();
            OrderBy = "Id";
        }
        #endregion
        #region FindAll
        public override async Task<ICollection<City>> FindAll()
        {
            CMDText = FINDALL_STMT;
            CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new CityMapToObject();
            return await base.FindAll();
        }
        #endregion

        #region FindAll(Pager)
        public async Task<IPager<City>> FindAll(IPager<City> pager)
        {
            CMDText = String.Format(FINDALLPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
            //CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new CityMapToObject();
            pager.Entities = await base.FindAll();
            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }
        #endregion

        #region FindByPK(Guid)
        public override async Task<City> FindByPK(IPrimaryKey pk)
        {
            CMDText = FINDBYPK_STMT;
            MapToObject = new CityMapToObject();
            return await base.FindByPK(pk);
        }
        #endregion

        public async Task<int> Add(City entity)
        {
            CMDText = ADD_STMT;
            MapFromObject = new CityMapFromObject();
            return await base.Add(entity, entity.PK);
        }
        public async Task<int> Update(City entity)
        {
            CMDText = UPDATE_STMT;
            MapFromObject = new CityMapFromObject();
            return await base.Update(entity, entity.PK);
        }

        public async Task<int> Delete(PrimaryKey pk)
        {
            CMDText = DELETE_STMT;
            return await base.Delete(pk);
        }


    }
}


