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
    public class BugService
    {
        private readonly IRepository _context;

        public BugService(IRepository dependency)
        {
            _context = dependency;
        }

        public List<Bugs> GetBugs()
        {
            return _context.Bugs.ToList();
        }
        public List<Bugs> GetBugsIncluded()
        {
            return _context.Bugs.Include(b => b.Status)
                              .Include(b => b.User)
                              .ToList();
        }
        public List<Bugs> GetBugsByStatus(string statusName)
        {
            return _context.Bugs.Where(b => b.Status.Name == statusName).ToList();
        }
        public List<Bugs> GetBugsAssignedToUser(string username)
        {
            return _context.Bugs.Where(b => b.User.Username == username).ToList();
        }
        public bool AssignBugToUser(int bugId, int userId)
        {
            try
            {
                var bug = _context.Bugs.Find(bugId);
                if (bug == null)
                {
                    return false;
                }
                bug.UserId = userId;
                return _context.SaveEntry(bug);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Bugs GetBugById(int? id)
        {
            return _context.Bugs.Where(b => b.Id == id).FirstOrDefault();
        }
        public bool AddBug(Bugs bug)
        {
            try
            {
                _context.Bugs.Add(bug);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBug(Bugs bug)
        {
            try
            {
                _context.SaveEntry(bug);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteBug(Bugs bug)
        {
            try
            {
                _context.Bugs.Remove(bug);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Async Methods
        public async Task<List<Bugs>> GetBugsAsync()
        {
            return await _context.Bugs.ToListAsync();
        }
        public async Task<List<Bugs>> GetBugsIncludedAsync()
        {
            return await _context.Bugs.Include(b => b.Status)
                              .Include(b => b.User)
                              .ToListAsync();
        }
        public async Task<List<Bugs>> GetBugsByStatusAsync(string statusName)
        {
            return await _context.Bugs.Where(b => b.Status.Name == statusName).ToListAsync();
        }
        public async Task<List<Bugs>> GetBugsAssignedToUserAsync(int userId)
        {
            return await _context.Bugs.Where(b => b.UserId == userId).ToListAsync();
        }
        public async Task<bool> AssignBugToUserAsync(int bugId, int userId)
        {
            try
            {
                var bug = _context.Bugs.Find(bugId);
                if (bug == null)
                {
                    return false;
                }
                bug.UserId = userId;
                return await _context.SaveEntryAsync(bug);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Bugs> GetBugByIdAsync(int? id)
        {
            return await _context.Bugs.FindAsync(id);
        }
        public async Task<bool> AddBugAsync(Bugs bug)
        {
            try
            {
                _context.Bugs.Add(bug);
                return await _context.SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateBugAsync(Bugs bug)
        {
            try
            {
                return await _context.SaveEntryAsync(bug);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteBugAsync(Bugs bug)
        {
            try
            {
                _context.Bugs.Remove(bug);
                return await _context.SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public DbSet<Statuses> GetStatusSet()
        {
            return _context.Statuses;
        }

        public DbSet<Users> GetUserSet()
        {
            return _context.Users;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}