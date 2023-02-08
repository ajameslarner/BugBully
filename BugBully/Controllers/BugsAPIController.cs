using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BugBully.Data;
using BugBully.Models;
using BugBully.Services;

namespace BugBully.Controllers
{
    public class BugsAPIController : ApiController
    {
        private readonly BugBullyContext _context;

        public BugsAPIController()
        {
            _context = new BugBullyContext();
        }

        // GET: api/BugsAPI
        public async Task<IHttpActionResult> GetBugs()
        {
            var bugs = await _context.Bugs.ToListAsync();
            return Ok(bugs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}