﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Jh.Abp.MenuManagement
{
    public class AuditLoggingRepository : EfCoreAuditLogRepository, IAuditLoggingRepository, ITransientDependency
    {
        public AuditLoggingRepository(IDbContextProvider<IAuditLoggingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<AuditLog[]> DeleteEntitysAsync(IQueryable<AuditLog> query, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entitys = query.ToArray();
            DbSet.RemoveRange(entitys);
            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
            }
            return entitys;
        }

        public virtual async Task<AuditLog[]> DeleteListAsync(Expression<Func<AuditLog, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entitys = DbSet.Where(predicate).ToArray();
            DbSet.RemoveRange(entitys);
            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
            }
            return entitys;
        }
    }
}
