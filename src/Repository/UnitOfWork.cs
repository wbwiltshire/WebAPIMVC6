using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeagullConsulting.WebAPIMVC6Website.Data;

namespace SeagullConsulting.WebAPIMVC6Website.Data.Repository
{
    public class UnitOfWork
    {
        private readonly SqlConnection connection;
        //private string connString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private string connString;
        private string SQLStatus = String.Empty;
        public IConfigurationRoot Configuration { get; set; }
        private readonly string appSettings = "appsettings.json";
        //protected static readonly ILog log = LogManager.GetLogger(typeof(UnitOfWork));
        private readonly ILogger logger;

        public UnitOfWork(ILogger l)
        {
            string CMDText = "BEGIN TRAN T1;";
            int rows;
            try
            {
                logger = l;
				connection = new SqlConnection(connString);
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(CMDText, connection))
                {
                    rows = cmd.ExecuteNonQuery();
                }
                TransactionCount = 0;
            }
            catch (SqlException ex)
            {
                SQLStatus = ex.ToString();
                logger.LogError(SQLStatus);
            }
        }
        private void GetConnectionString()
        {
            //Get a reference to the config file
            var builder = new ConfigurationBuilder()
                .AddJsonFile(appSettings);
            Configuration = builder.Build();
            connString = Configuration.GetSection("Data:CustomerConnection:ConnectionString").Value;
            logger.LogInformation("ConnectionString: " + connString);
        }

        internal SqlConnection Connection
        {
            get { return connection; }
        }
        internal ILogger Logger
        {
            get { return logger; }
        }
        public bool Enlist() 
        {
            TransactionCount++;
            return true;
        }
        public int TransactionCount { get; set; }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //We'll close here, if UOW.  Otherwise, close in RepositoryBase
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
