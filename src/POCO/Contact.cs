using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SeagullConsulting.WebAPIMVC6Website.Data.Interfaces;

namespace SeagullConsulting.WebAPIMVC6Website.Data.POCO
{
    public class Contact
    {
        public PrimaryKey PK { get; set; }
        public int Id { 
            get { return (int) PK.Key; }
            set { PK.Key = (int) value; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Notes { get; set; }
        public string ZipCode { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string CellPhone { get; set; }
        public string EMail { get; set; }
        public int CityId { get; set; }
        //Properties managed by the architecture
        public bool Active { get; set; }
        public DateTime ModifiedDt { get; set; }
        public DateTime CreateDt { get; set; }
        public Contact()
        {
            PK = new PrimaryKey() { Key = -1, IsIdentity = true };
        }
        public string ToPrint()
        {
            return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt);
        }

        //Relation properties
        public City City { get; set; }
    }

    public class ContactMapToObject : MapToObjectBase<Contact>, IMapToObject<Contact>
	{
        public override Contact Execute(SqlDataReader reader)
		{
            Contact contact = new Contact();
            try
            {
                contact.Id = reader.GetInt32(0);
                contact.FirstName = reader.GetString(1);
                contact.LastName = reader.GetString(2);
                if (reader.IsDBNull(3) == false)
                    contact.Address1 = reader.GetString(3);
                if (reader.IsDBNull(4) == false)
                    contact.Address2 = reader.GetString(4);
                if (reader.IsDBNull(5) == false)
                    contact.Notes = reader.GetString(5);
                if (reader.IsDBNull(6) == false)
                    contact.ZipCode = reader.GetString(6);
                if (reader.IsDBNull(7) == false)
                    contact.HomePhone = reader.GetString(7);
                if (reader.IsDBNull(8) == false)
                    contact.WorkPhone = reader.GetString(8);
                if (reader.IsDBNull(9) == false)
                    contact.CellPhone = reader.GetString(9);
                if (reader.IsDBNull(10) == false)
                    contact.EMail = reader.GetString(10);
                contact.CityId = reader.GetInt32(11);
                contact.Active = reader.GetBoolean(12);
                contact.ModifiedDt = reader.GetDateTime(13);
                contact.CreateDt = reader.GetDateTime(14);
            }
            catch (Exception ex)
            {
                SQLStatus = ex.ToString();
                //logger.LogError(SQLStatus);
            }
            return contact;
		}
    }

    public class ContactMapToObjectView : MapToObjectBase<Contact>, IMapToObject<Contact>
    {
        public override Contact Execute(SqlDataReader reader)
        {
            Contact contact = new Contact();
            try
            {
                contact.Id = reader.GetInt32(0);
                contact.FirstName = reader.GetString(1);
                contact.LastName = reader.GetString(2);
                if (reader.IsDBNull(3) == false)
                    contact.Address1 = reader.GetString(3);
                if (reader.IsDBNull(4) == false)
                    contact.Address2 = reader.GetString(4);
                if (reader.IsDBNull(5) == false)
                    contact.Notes = reader.GetString(5);
                if (reader.IsDBNull(6) == false)
                    contact.ZipCode = reader.GetString(6);
                if (reader.IsDBNull(7) == false)
                    contact.HomePhone = reader.GetString(7);
                if (reader.IsDBNull(8) == false)
                    contact.WorkPhone = reader.GetString(8);
                if (reader.IsDBNull(9) == false)
                    contact.CellPhone = reader.GetString(9);
                if (reader.IsDBNull(10) == false)
                    contact.EMail = reader.GetString(10);
                contact.CityId = reader.GetInt32(11);
                contact.City = new City { PK = new PrimaryKey { Key = contact.CityId, IsIdentity = true }, Name = reader.GetString(12), State = new State { PK = new PrimaryKey { Key = reader.GetString(13), IsIdentity = false }, Name = reader.GetString(14) } };
                contact.Active = reader.GetBoolean(15);
                contact.ModifiedDt = reader.GetDateTime(16);
                contact.CreateDt = reader.GetDateTime(17);
            }
            catch (Exception ex)
            {
                SQLStatus = ex.ToString();
                //logger.LogError(SQLStatus);
            }
            return contact;
        }
    }

    public class ContactMapFromObject : MapFromObjectBase<Contact>, IMapFromObject<Contact>
    {
        public override void Execute(Contact contact, SqlCommand cmd)
        {
            SqlParameter parm;

            try
            {
                parm = new SqlParameter("@p1", contact.FirstName);
                cmd.Parameters.Add(parm);
                parm = new SqlParameter("@p2", contact.LastName);
                cmd.Parameters.Add(parm);
                parm = new SqlParameter("@p3", contact.Address1);
                cmd.Parameters.Add(parm);
                if (contact.Address2 == null)
                    parm = new SqlParameter("@p4", DBNull.Value);
                else
                    parm = new SqlParameter("@p4", contact.Address2);
                cmd.Parameters.Add(parm);
                if (contact.Notes == null)
                    parm = new SqlParameter("@p5", DBNull.Value);
                else
                    parm = new SqlParameter("@p5", contact.Notes);
                cmd.Parameters.Add(parm);
                if (contact.ZipCode == null)
                    parm = new SqlParameter("@p6", DBNull.Value);
                else
                    parm = new SqlParameter("@p6", contact.ZipCode);
                cmd.Parameters.Add(parm);
                if (contact.HomePhone == null)
                    parm = new SqlParameter("@p7", DBNull.Value);
                else
                    parm = new SqlParameter("@p7", contact.HomePhone);
                cmd.Parameters.Add(parm);
                if (contact.WorkPhone == null)
                    parm = new SqlParameter("@p8", DBNull.Value);
                else
                    parm = new SqlParameter("@p8", contact.WorkPhone);
                cmd.Parameters.Add(parm);
                if (contact.CellPhone == null)
                    parm = new SqlParameter("@p9", DBNull.Value);
                else
                    parm = new SqlParameter("@p9", contact.CellPhone);
                cmd.Parameters.Add(parm);
                if (contact.EMail == null)
                    parm = new SqlParameter("@p10", DBNull.Value);
                else
                    parm = new SqlParameter("@p10", contact.EMail);
                cmd.Parameters.Add(parm);
                parm = new SqlParameter("@p11", contact.CityId);
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
