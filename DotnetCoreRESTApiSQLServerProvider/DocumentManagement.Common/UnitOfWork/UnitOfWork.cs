using DocumentManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.Common.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UnitOfWork<TContext>> _logger;
        public UnitOfWork(
            TContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UnitOfWork<TContext>> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public int Save()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    SetModifiedInformation();
                    var retValu = _context.SaveChanges();
                    transaction.Commit();
                    return retValu;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError("500", e);
                    return 0;
                }
            }
        }

        public bool Migration()
        {
            _context.Database.Migrate();
            return true;

        }
        public async Task<int> SaveAsync()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    SetModifiedInformation();
                    var val = await _context.SaveChangesAsync();
                    transaction.Commit();
                    return val;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError("500", e);
                    return 0;
                }
            }
        }
        public TContext Context => _context;
        public void Dispose()
        {
            _context.Dispose();
        }

        private void SetModifiedInformation()
        {
            foreach (var entry in Context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                    {
                        entry.Entity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.IsDeleted)
                    {
                        if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                        {
                            entry.Entity.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                        }
                        entry.Entity.DeletedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                        {
                            entry.Entity.ModifiedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                        }
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
