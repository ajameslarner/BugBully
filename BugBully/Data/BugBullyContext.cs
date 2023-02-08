using BugBully.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BugBully.Data
{
    public interface IRepository
    {
        DbSet<Users> Users { get; set; }
        DbSet<Statuses> Statuses { get; set; }
        DbSet<Bugs> Bugs { get; set; }

        bool Save();
        bool SaveEntry<T>(T entity);
        Task<bool> SaveAsync();
        Task<bool> SaveEntryAsync<T>(T entity);
        void Dispose();
    }

    public class BugBullyContext : DbContext, IRepository
    {
        public BugBullyContext() : base("name=BugBullyDbConnection") {}

        public DbSet<Users> Users { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<Bugs> Bugs { get; set; }

        public bool SaveEntry<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
            return Save();
        }
        public async Task<bool> SaveEntryAsync<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
            return await SaveAsync();
        }
        public bool Save()
        {
            try
            {
                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SaveAsync()
        {
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}