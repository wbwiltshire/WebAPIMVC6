using System;
using System.Collections.Generic;
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
    public class ContactRepository : RepositoryBase<Contact>, IRepository<Contact>
    {

        private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM Contact WHERE Active=1";
        private const string FINDALL_STMT = "SELECT Id,FirstName,LastName,Address1,Address2,Notes,ZipCode,HomePhone,WorkPhone,CellPhone,EMail,CityId,Active,ModifiedDt,CreateDt FROM Contact WHERE Active=1";
        private const string FINDALLVIEW_STMT = "SELECT c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, c2.Name as CityName, StateId, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt FROM Contact c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) WHERE c1.Active=1";
        //private const string FINDALLPAGER_STMT = "SELECT TOP({0}) Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt FROM (SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt, ROW_NUMBER() OVER (ORDER BY {1}) AS [rc] FROM Contact) as c WHERE rc > {2}";
        private const string FINDALLPAGER_STMT = "SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt FROM Contact WHERE Active=1 ORDER BY Id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;";
        //private const string FINDALLVIEWPAGER_STMT = "SELECT TOP({0}) c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId,  c2.Name as CityName, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt FROM (SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt, ROW_NUMBER() OVER (ORDER BY {1}) AS [rc] FROM Contact) as c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) WHERE rc > {2}";
        private const string FINDALLVIEWPAGER_STMT = "SELECT c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId,  c2.Name as CityName, s.Id, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt FROM Contact c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) WHERE c1.Active=1 ORDER BY c1.Id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;";
        private const string FINDBYPK_STMT = "SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt FROM Contact WHERE Id =@pk";
        private const string FINDBYPKVIEW_STMT = "SELECT c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, c2.Name as CityName, s.Id, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt FROM Contact c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) WHERE c1.Id =@pk ORDER BY c1.Id ";
        private const string ADD_STMT = "INSERT INTO Contact (FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, 1, GETDATE(), GETDATE()); SELECT CAST(scope_identity() AS int)";
        private const string UPDATE_STMT = "UPDATE Contact SET FirstName=@p1, LastName=@p2, Address1=@p3, Address2=@p4, Notes=@p5, ZipCode=@p6, HomePhone=@p7, WorkPhone=@p8, CellPhone=@p9, EMail=@p10, CityId=@p11, Active=1, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string DELETE_STMT = "UPDATE Contact SET Active=0, ModifiedDt=GETDATE() WHERE Id =@pk";
        private const string ORDERBY_STMT = " ORDER BY ";

        #region ctor
        //Default constructor calls the base ctor
        public ContactRepository(ILogger l, IConfigurationRoot c) :
            base(l, c)
        { Init(); }
        public ContactRepository(UnitOfWork uow) :
            base(uow)
        { Init(); }


        private void Init()
        {
            //Mapper = new ContactMapper();
            OrderBy = "Id";
        }
        #endregion

        public override async Task<ICollection<Contact>> FindAll()
        {
            CMDText = FINDALL_STMT;
            CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new ContactMapToObject();
            return await base.FindAll();
        }

        public async Task<IPager<Contact>> FindAll(IPager<Contact> pager)
        {
            CMDText = String.Format(FINDALLPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
            //CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new ContactMapToObject();
            pager.Entities = await base.FindAll();
            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }

        public async Task<ICollection<Contact>> FindAllView()
        {
            CMDText = FINDALLVIEW_STMT;
            CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new ContactMapToObjectView();
            return await base.FindAll();        
        }

        public async Task<IPager<Contact>> FindAllView(IPager<Contact> pager)
        {
            CMDText = String.Format(FINDALLVIEWPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
            //CMDText += ORDERBY_STMT + OrderBy;
            MapToObject = new ContactMapToObjectView();
            pager.Entities = await base.FindAll();
            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }

        public override async Task<Contact> FindByPK(IPrimaryKey pk)
        {
            CMDText = FINDBYPK_STMT;
            MapToObject = new ContactMapToObject();
            return await base.FindByPK(pk);
        }

        public async Task<Contact> FindByPKView(IPrimaryKey pk)
        {
            CMDText = FINDBYPKVIEW_STMT;
            MapToObject = new ContactMapToObjectView();
            return await base.FindByPK(pk);
        }

        public async Task<int> Add(Contact entity)
        {
            CMDText = ADD_STMT;
            MapFromObject = new ContactMapFromObject();
            return await base.Add(entity, entity.PK);
        }
        public async Task<int> Update(Contact entity)
        {
            CMDText = UPDATE_STMT;
            MapFromObject = new ContactMapFromObject();
            return await base.Update(entity, entity.PK);
        }

        public async Task<int> Delete(PrimaryKey pk)
        {
            CMDText = DELETE_STMT;
            return await base.Delete(pk);
        }
    }
}
