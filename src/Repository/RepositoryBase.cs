using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeagullConsulting.WebAPIMVC6Website.Data;
using SeagullConsulting.WebAPIMVC6Website.Data.Interfaces;

namespace SeagullConsulting.WebAPIMVC6Website.Data.Repository
{
 
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        //private string connString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private string connString;
 
        protected SqlConnection connection;
        private UnitOfWork unitOfWork;
        //private readonly string appSettings = "appsettings.json";
        protected IConfigurationRoot Configuration { get; set; }
        //protected static readonly ILog log = LogManager.GetLogger(typeof(RepositoryBase<TEntity>));
        private ILogger logger;

        #region ctor
        //ctor with no unit of work necessary
        protected RepositoryBase(ILogger l, IConfigurationRoot c)
        {
            Configuration = c;
            logger = l;
            GetConnectionString();
            connection = new SqlConnection(connString);
        }
        //ctor with unit of work
        protected RepositoryBase(UnitOfWork uow)
        {
            unitOfWork = uow;
            connection = unitOfWork.Connection;
            logger = unitOfWork.Logger;
        }
        #endregion

        private void GetConnectionString()
        {
            //Get a reference to the config file
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile(appSettings);
            //Configuration = builder.Build();
            connString = Configuration.GetSection("Data:CustomerConnection:ConnectionString").Value;
            logger.LogInformation("ConnectionString: " + connString);
        }

        public string SQLStatus { get; set; }
        public string OrderBy { get; set; }
        protected string CMDText { get; set; }
        //protected MapperBase<TEntity> Mapper { get; set; }
        protected MapToObjectBase<TEntity> MapToObject { get; set; }
        protected MapFromObjectBase<TEntity> MapFromObject { get; set; }

        private async Task OpenConnection()
        {
            try
            {
                await connection.OpenAsync();
                SQLStatus = "INITDB - OK";
                logger.LogInformation("Repository Connection opened.");
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
            }
        }

        protected async Task<int> FindAllCount()
        {
            object cnt;

            if (connection.State != ConnectionState.Open)
                await OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    cnt = await cmd.ExecuteScalarAsync();
                    logger.LogInformation("FindAllCount complete.");
                    if (cnt != null)
                        return Convert.ToInt32(cnt);
                    else
                        return 0;
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
                return 0;
            }
        }

        public virtual async Task<ICollection<TEntity>> FindAll()
        {
            if (connection.State != ConnectionState.Open)
                await OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        ICollection<TEntity> entities = new List<TEntity>();
                        while (await reader.ReadAsync())
                        {
                            entities.Add(MapToObject.Execute(reader));
                        }
                        logger.LogInformation(String.Format("FindAll complete for {0} entity.", typeof(TEntity)));
                        return entities;
                    }
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
                return null;
            }
        }

        public virtual async Task<TEntity> FindByPK(IPrimaryKey pk)
        {
            TEntity entity = null;

            if (connection.State != ConnectionState.Open)
                await OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                            entity = MapToObject.Execute(reader);
                        else
                            entity = null;
                        logger.LogInformation(String.Format("FindByPK complete for {0} entity.", typeof(TEntity)));
                    }
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
                return null;
            }

            return entity;
        }

        protected async Task<int> Add(TEntity entity, PrimaryKey pk)
        {
            int result = 0;
            object cnt;

            if (connection.State != ConnectionState.Open)
                await OpenConnection();
            
            //Check for Identity column
            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    MapFromObject.Execute(entity, cmd);
                    if (pk.IsIdentity)
                    {
                        //returns PK
                        cnt = await cmd.ExecuteScalarAsync();
                        if (cnt != null)
                            result = Convert.ToInt32(cnt);
                        if (unitOfWork != null) unitOfWork.Enlist();
                    }
                    else
                    {
                        //returns rows affected
                        cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                        result = await cmd.ExecuteNonQueryAsync();
                        if (unitOfWork != null) unitOfWork.Enlist();
                    }
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
            }
            logger.LogInformation(String.Format("Add complete for {0} entity.", typeof(TEntity)));
            return result;
        }
 
        protected async Task<int> Update(TEntity entity, IPrimaryKey pk)
        {
            int rows = 0;

            if (connection.State != ConnectionState.Open)
                await OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    MapFromObject.Execute(entity, cmd);
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    rows = await cmd.ExecuteNonQueryAsync();
                    if (unitOfWork != null) unitOfWork.Enlist();
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
            }
            logger.LogInformation(String.Format("Update complete for {0} entity.", typeof(TEntity)));
            return rows;
        }
 
        protected async Task<int> Delete(IPrimaryKey pk)
        {
            int rows = 0;

            if (connection.State != ConnectionState.Open)
                await OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    rows = await cmd.ExecuteNonQueryAsync();
                    if (unitOfWork != null) unitOfWork.Enlist();
                }
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
            }
            logger.LogInformation(String.Format("Delete complete for {0} entity.", typeof(TEntity)));
            return rows;
        }

        public async Task<bool> Save()
        {
            CMDText = "COMMIT TRAN T1;";
            bool status = false;
            int rows;

            //Nothing to do if no Unit of Work or no transactions have enlisted
            if ((unitOfWork != null) && (unitOfWork.TransactionCount > 0))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                    {
                        rows = await cmd.ExecuteNonQueryAsync();
                        status = true;
                        unitOfWork.TransactionCount = 0;
                        logger.LogInformation("Save complete and unit of work committed.");
                    }
                }
                catch (SqlException ex)
                {
                    SQLStatus = ex.ToString();
                    logger.LogError(SQLStatus);
                }
            }
            else
            {
                status = true;
                logger.LogInformation("Save ignored, because no outstanding unit of work exists.");
            }
            return status;
        }

        public async Task<bool> Rollback()
        {
            CMDText = "ROLLBACK TRAN T1;";
            bool status = false;
            int rows;

            //TODO: Do I need to check connection state here?
            //if (connection.State != ConnectionState.Open)
            //    await OpenConnection();

            if ((unitOfWork != null) && (unitOfWork.TransactionCount > 0))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                    {
                        rows = await cmd.ExecuteNonQueryAsync();
                        status = true;
                        unitOfWork.TransactionCount = 0;
                        logger.LogInformation("Rollback complete and unit of work cleared.");
                    }
                }
                catch (SqlException ex)
                {
                    SQLStatus = ex.ToString();
                    logger.LogError(SQLStatus);
                }
            }
            else
            {
                status = true;
                logger.LogInformation("Rollback ignored, because no outstanding unit of work exists.");
            }
            return status;
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //We'll close here, if no UOW.  Otherwise, close when UOW disposed
            if (unitOfWork == null)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if (connection != null)
                            connection.Close();
                        logger.LogInformation("Repository Connection closed.");
                    }
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
