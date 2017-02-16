using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeagullConsulting.WebAPIMVC6Website.Data.POCO;

namespace SeagullConsulting.WebAPIMVC6Website.Web.UI.Models
{
    public class ContactsView
    {
        public IEnumerable<Contact> Contacts { get; set; }
        public int TotalItems { get; set; }
    }
}
