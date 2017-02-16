using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using SeagullConsulting.WebAPIMVC6Website.Data.POCO;
using SeagullConsulting.WebAPIMVC6Website.Data.Repository;
using SeagullConsulting.WebAPIMVC6Website.Web.UI.Models;

namespace SeagullConsulting.WebAPIMVC6Website.Web.UI.APIControllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private ContactRepository repository;
        private ILogger logger;

        public ContactController(ILogger<ContactController> l, IConfigurationRoot config)
        {
            logger = l;
            repository = new ContactRepository(l, config);
        }
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            IEnumerable<Contact> entities = await repository.FindAllView();
            return entities;
        }

        [HttpGet("Paged")]
        public async Task<ContactsView> Paged(int page, int pageSize, string sortColumn, string direction)
        {
            IEnumerable<Contact> sortedContacts = null;
            ICollection<Contact> contacts = await repository.FindAllView();
            string sortParms = String.Format("{0} {1}", sortColumn, direction.ToUpper());

            if (pageSize > contacts.Count())
                pageSize = contacts.Count();
            int start = (page - 1) * pageSize;
            int itemCount = start + pageSize;
            if (itemCount > contacts.Count())
                itemCount = contacts.Count();

            sortedContacts = contacts.AsQueryable().OrderBy<Contact>(sortParms).AsEnumerable<Contact>();

            ContactsView cv = new ContactsView()
            {
                TotalItems = contacts.Count(),
                Contacts = sortedContacts.Take(itemCount).Skip(start)
            };

            return cv;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
