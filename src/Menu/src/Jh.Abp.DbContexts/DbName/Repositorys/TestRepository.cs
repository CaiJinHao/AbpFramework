using Dapper;
using Jh.Abp.MenuManagement.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Jh.Abp.DbContexts.DbName.Repositorys
{
    public class TestRepository : DapperRepository<DbNameDbContext>, ITestRepository, ITransientDependency
    {
        public TestRepository(IDbContextProvider<DbNameDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IEnumerable<string>> GetEmailsAsync()
        {
            var querySql = @"SELECT Email FROM AbpUsers";
            var data= await DbConnection.QueryAsync<string>(querySql, transaction: DbTransaction);
            return data;
        }
    }
}
