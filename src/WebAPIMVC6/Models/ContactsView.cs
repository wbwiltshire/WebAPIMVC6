using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeagullConsulting.WebAPIMVC6Website.Data.POCO;

namespace SeagullConsulting.WebAPIMVC6Website.Web.UI.Models
{
    public class ContactsView
    {
        public IEnumerable<ContactView> Contacts { get; set; }
        public int TotalItems { get; set; }
    }

    public class ContactView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Active { get; set; }
    }
}
