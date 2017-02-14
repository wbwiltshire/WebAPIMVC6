/******************************************************************************************************
 *This class was generated on 04/20/2014 09:31:37 using Repository Builder version 0.9. *
 *The class was generated from Database: BACS and Table: State.  *
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

    public class StateRepository : RepositoryBase<State>, IRepository<State>
    {
        private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM State WHERE Active=1"; 
        private const string FINDALL_STMT = "SELECT Id,Name,Active,ModifiedDt,CreateDt FROM State WHERE Active=1";
        //private const string FINDALLPAGER_STMT = "SELECT TOP({0}) Id, Name, Active, ModifiedDt, CreateDt FROM (SELECT Id, Name, Active, ModifiedDt, CreateDt, ROW_NUMBER() OVER (ORDER BY {1}) AS [rc] FROM State) AS s WHERE rc > {2}";
        private const string FINDALLPAGER_STMT = "SELECT Id,Name,Active,ModifiedDt,CreateDt FROM State WHERE Active=1 ORDER BY Id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY";
        private const string FINDBYPK_STMT = "SELECT Id, Name, Active, ModifiedDt, CreateDt FROM State WHERE Id =@pk";
        private const string ADD_STMT = "INSERT INTO State (Id, Name, Active, ModifiedDt, CreateDt) VALUES (@pk, @p1, 1, GETDATE(), GETDATE())";
        private const string UPDATE_STMT = "UPDATE State SET Name=@p1, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string DELETE_STMT = "UPDATE State SET Active=0, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string ORDERBY_STMT = " ORDER BY ";

        #region ctor
        //Default constructor calls the base ctor
        public StateRepository(ILogger l, IConfigurationRoot c) :
            base(l, c)
        { Init(); }
        public StateRepository(UnitOfWork uow) :
            base(uow)
        { Init(); }

        private void Init()
        {
            //Mapper = new StateMapper();
            OrderBy = "Id";
        }
        #endregion
        #region FindAll
        public override async Task<ICollection<State>> FindAll()
        {
            CMDText = FINDALL_STMT;
            CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new StateMapToObject();
            return await base.FindAll();
        }
        #endregion

        #region FindAll(Pager)
        public async Task<IPager<State>> FindAll(IPager<State> pager)
        {
            CMDText = String.Format(FINDALLPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
            //CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new StateMapToObject();
            pager.Entities = await base.FindAll();
            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }
        #endregion

        #region FindByPK(IPrimaryKey pk)
        public override async Task<State> FindByPK(IPrimaryKey pk)
        {
            CMDText = FINDBYPK_STMT;
            MapToObject = new StateMapToObject();
            return await base.FindByPK(pk);
        }
        #endregion

        public async Task<int> Add(State entity)
        {
            CMDText = ADD_STMT;
            MapFromObject = new StateMapFromObject();
            return await base.Add(entity, entity.PK);
        }
        public async Task<int> Update(State entity)
        {
            CMDText = UPDATE_STMT;
            MapFromObject = new StateMapFromObject();
            return await base.Update(entity, entity.PK);
        }

        public async Task<int> Delete(PrimaryKey pk)
        {
            CMDText = DELETE_STMT;
            return await base.Delete(pk);
        }

    }
}


