using BugBully.Data;
using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BugBully.Models;
using System.Data.Entity;
using System.Linq;

namespace BugBully.Services
{
    public class APIService
    {
        private readonly IRepository _context;

        public APIService(IRepository dependency)
        {
            _context = dependency;
        }
        public async Task<List<Bugs>> GetBugsAsync()
        {
            return await _context.Bugs.ToListAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}