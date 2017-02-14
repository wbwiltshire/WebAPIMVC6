/******************************************************************************************************
 *This class was generated on 04/30/2014 09:00:34 using Repository Builder version 0.9. *
 *The class was generated from Database: Customer and Table: City.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SeagullConsulting.WebAPIMVC6Website.Data;
using SeagullConsulting.WebAPIMVC6Website.Data.Interfaces;

namespace SeagullConsulting.WebAPIMVC6Website.Data.POCO
{

    public class City
    {

        public PrimaryKey PK { get; set; }
        public int Id
        {
            get { return (int)PK.Key; }
            set { PK.Key = (int)value; }
        }
        public string Name { get; set; }
        public string StateId { get; set; }
        public bool Active { get; set; }
        public DateTime ModifiedDt { get; set; }
        public DateTime CreateDt { get; set; }
        public City()
        {
            PK = new PrimaryKey() { Key = -1, IsIdentity = true };
        }
        public string ToPrint()
        {
            return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|", Id, Name, StateId, Active, ModifiedDt, CreateDt);
        }

        //Relation properties
        public State State { get; set; }
    }

    public class CityMapToObject : MapToObjectBase<City>, IMapToObject<City>
    {
        public override City Execute(SqlDataReader reader)
        {
            City city = new City();

            try 
            {
            city.Id = reader.GetInt32(0);
            city.Name = reader.GetString(1);
            city.StateId = reader.GetString(2);
            city.Active = reader.GetBoolean(3);
            city.ModifiedDt = reader.GetDateTime(4);
            city.CreateDt = reader.GetDateTime(5);
            }
            catch (Exception ex)
            {
                SQLStatus = ex.ToString();
                //logger.LogError(SQLStatus);
            }
            return city;
        }
    }
    public class CityMapToObjectView : MapToObjectBase<City>, IMapToObject<City>
    {
        public override City Execute(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
    public class CityMapFromObject : MapFromObjectBase<City>, IMapFromObject<City>
    {
        public override void Execute(City city, SqlCommand cmd)
        {
            SqlParameter parm;
            try
            {
                parm = new SqlParameter("@p1", city.Name);
                cmd.Parameters.Add(parm);
                parm = new SqlParameter("@p2", city.StateId);
                cmd.Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                SQLStatus = ex.ToString();
                //logger.LogError(SQLStatus);
            }
        }
    }
}


