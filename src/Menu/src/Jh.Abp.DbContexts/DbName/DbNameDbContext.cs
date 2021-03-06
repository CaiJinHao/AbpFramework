using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Jh.Abp.DbContexts.DbName
{
    [ConnectionStringName("Default")]
    public class DbNameDbContext : AbpDbContext<DbNameDbContext>
    {
        public DbNameDbContext(DbContextOptions<DbNameDbContext> options) : base(options)
        {
        }
    }
}
