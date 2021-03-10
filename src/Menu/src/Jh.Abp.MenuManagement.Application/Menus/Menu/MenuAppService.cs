﻿using Jh.Abp.Application.Contracts.Extensions;
using Jh.Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Jh.Abp.MenuManagement.Menus
{
    public class MenuAppService
        : CrudApplicationService<Menu, MenuDto, MenuDto, Guid, MenuRetrieveInputDto, MenuCreateInputDto, MenuUpdateInputDto, MenuDeleteInputDto>
        , IMenuAppService
    {
        private readonly IMenuRepository menuRepository;
        private readonly IMenuDapperRepository MenuDapperRepository;

        private readonly IMenuAndRoleMapRepository menuAndRoleMapRepository;
        private readonly IMenuAndRoleMapAppService menuAndRoleMapAppService;

        public MenuAppService(IMenuRepository repository,
            IMenuDapperRepository menuDapperRepository, 
            IMenuAndRoleMapRepository _menuAndRoleMapRepository,
            IMenuAndRoleMapAppService _menuAndRoleMapAppService,
            IMenuAndRoleMapDomainService _menuAndRoleMapDomainService) : base(repository)
        {
            menuRepository = repository;
            MenuDapperRepository = menuDapperRepository;
            menuAndRoleMapRepository = _menuAndRoleMapRepository;
            menuAndRoleMapAppService = _menuAndRoleMapAppService;
        }

        [UnitOfWork]
        public override async Task<Menu> CreateAsync(MenuCreateInputDto inputDto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var entity = await base.CreateAsync(inputDto, true, cancellationToken);
            if (inputDto.RoleIds != null && inputDto.RoleIds.Length > 0)
            {
                foreach (var roleid in inputDto.RoleIds)
                {
                    entity.AddMenuRoleMap(roleid);
                }
            }
            return entity;
        }

        private IEnumerable<Menu> EnumerableCreateAsync(MenuCreateInputDto[] inputDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var item in inputDtos)
            {
                yield return this.CreateAsync(item, autoSave, cancellationToken).Result;
            }
        }

        [UnitOfWork]
        public override Task<Menu[]> CreateAsync(MenuCreateInputDto[] inputDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(EnumerableCreateAsync(inputDtos, autoSave, cancellationToken).ToArray());
        }

        [UnitOfWork]
        public override async Task<Menu> DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            /*var data = MenuDapperRepository.GetDapperListAsync().Result;
            if (data != null)
            {
                var c = menuRepository.GetDapperListAsync().Result;
            }*/
            // 多数据库连接测试
            /*var data= await testRepository.GetEmailsAsync();
            if (data!=null)
            {

            }*/
            var entity = await base.DeleteAsync(id, autoSave, cancellationToken);
            await menuAndRoleMapRepository.DeleteListAsync(a => a.MenuId == entity.Id).ConfigureAwait(false);
            return entity;
        }

        [UnitOfWork]
        public override async Task<Menu[]> DeleteAsync(Guid[] keys, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entitys = await base.DeleteAsync(keys, autoSave, cancellationToken);
            await menuAndRoleMapRepository.DeleteListAsync(a => keys.Contains(a.MenuId)).ConfigureAwait(false);
            return entitys;
        }

        [UnitOfWork]
        public override async Task<Menu[]> DeleteAsync(MenuDeleteInputDto deleteInputDto, string methodStringType = ObjectMethodConsts.Equals, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entitys = await base.DeleteAsync(deleteInputDto, autoSave:autoSave, cancellationToken:cancellationToken);
            await menuAndRoleMapRepository.DeleteListAsync(a => entitys.Select(b => b.Id).Contains(a.MenuId)).ConfigureAwait(false);
            return entitys;
        }
    }
}
