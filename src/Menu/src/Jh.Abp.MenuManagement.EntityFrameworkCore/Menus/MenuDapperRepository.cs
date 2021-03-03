﻿using Jh.Abp.MenuManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Dapper;
using Jh.Abp.MenuManagement.Menus;

namespace Jh.Abp.MenuManagement.Menus
{
    /*
     使用该类的模块需要引用typeof(AbpDapperModule)
     */
    public class MenuDapperRepository : DapperRepository<MenuManagementDbContext>, IMenuDapperRepository, ITransientDependency
    {
        public MenuDapperRepository(IDbContextProvider<MenuManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IEnumerable<Menu>> GetDapperListAsync()
        {
            var querySql = @"select a.Id,a.Code,a.Name,a.Icon,a.Sort,a.ParentCode,a.Url,a.Description,a.ConcurrencyStamp,a.CreationTime,a.CreatorId,a.LastModificationTime,a.LastModificationTime,a.LastModifierId,a.IsDeleted,a.DeleterId,a.DeletionTime from SysMenu a where a.Code = @Code";
            return await DbConnection.QueryAsync<Menu>(querySql, param: new { Code = "A01" }, transaction: DbTransaction);
        }
    }
}
