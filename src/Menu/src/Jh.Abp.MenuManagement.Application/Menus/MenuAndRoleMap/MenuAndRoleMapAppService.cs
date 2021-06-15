﻿using Jh.Abp.Extensions;
using Jh.Abp.MenuManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Jh.Abp.MenuManagement
{
    public class MenuAndRoleMapAppService
        : CrudApplicationService<MenuAndRoleMap, MenuAndRoleMapDto, MenuAndRoleMapDto, Guid, MenuAndRoleMapRetrieveInputDto, MenuAndRoleMapCreateInputDto, MenuAndRoleMapUpdateInputDto, MenuAndRoleMapDeleteInputDto>,
        IMenuAndRoleMapAppService
    {
        protected IMenuPermissionMapAppService menuPermissionMapAppService => LazyServiceProvider.LazyGetRequiredService<IMenuPermissionMapAppService>();

        private readonly IMenuRepository MenuRepository;
        public IdentityUserManager MyUserManager { get; set; }

        private readonly IMenuAndRoleMapRepository MenuAndRoleMapRepository;
        public MenuAndRoleMapAppService(IMenuAndRoleMapRepository repository, IMenuRepository menuRepository) : base(repository)
        {
            MenuAndRoleMapRepository = repository;
            MenuRepository = menuRepository;
        }

        public override Task<MenuAndRoleMap> CreateAsync(MenuAndRoleMapCreateInputDto inputDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<MenuAndRoleMap[]> CreateAsync(MenuAndRoleMapCreateInputDto[] inputDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<MenuAndRoleMap[]> CreateV2Async(MenuAndRoleMapCreateInputDto inputDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await menuPermissionMapAppService.UpdateAsync(inputDto.ProviderName, inputDto.ProviderKey, inputDto.PermissionNames);
            return await crudRepository.CreateAsync(GetCreateEnumerableAsync(inputDto).ToArray());
        }

        protected virtual IEnumerable<MenuAndRoleMap> GetCreateEnumerableAsync(MenuAndRoleMapCreateInputDto inputDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var roleid in inputDtos.RoleIds)
            {
                //删除所有角色的权限
                DeleteAsync(new MenuAndRoleMapDeleteInputDto() { RoleId = roleid }).Wait();
                foreach (var menuid in inputDtos.MenuIds)
                {
                    yield return new MenuAndRoleMap(menuid, roleid);
                }
            }
        }

        public virtual async Task<IEnumerable<MenusNavDto>> GetMenusNavTreesAsync()
        {
            //查看CurrentUser.Roles 是的值是否为guid ,只能用一个角色的权限渲染菜单
            //var roles = CurrentUser.FindClaims(Common.Extensions.JhJwtClaimTypes.RoleId).Select(a => new Guid(a.Value)).ToList();
            var roles = await GetRolesAsync();
            //查看CurrentUser.Roles 是的值是否为guid ,只能用一个角色的权限渲染菜单
            var auth_menus_id = crudRepository.Where(a => roles.Contains(a.RoleId)).Select(a => a.MenuId).ToList();

            //按照前端要求字段返回
            var auth_menus = await MenuRepository.Where(m => auth_menus_id.Contains(m.Id))
                .Select(a => new MenusNavDto() { id = a.Code, icon = a.Icon, parent_id = a.ParentCode, sort = a.Sort, title = a.Name, url = a.Url}).ToListAsync();

            //返回多个根节点
            return await GetMenusTreeAsync(auth_menus);
        }

        protected virtual async Task<IEnumerable<Guid>> GetRolesAsync()
        {
            var userid = CurrentUser.Id;
            var user = await MyUserManager.GetByIdAsync((Guid)userid);
            return user.Roles.Select(a => a.RoleId);
        }

        private MenuAndRoleMapTreeAllRetrieveInputDto menuAndRoleMapTreeAllRetrieveInputDto { get; set; }
        public virtual async Task<IEnumerable<MenusTreeDto>> GetMenusTreesAsync(MenuAndRoleMapTreeAllRetrieveInputDto input)
        {
            menuAndRoleMapTreeAllRetrieveInputDto = input;
            var auth_menus_id = crudRepository.Where(a => a.RoleId == input.RoleId).Select(a => a.MenuId).ToList();

            var resutlMenus = await MenuRepository.Select(a =>
                new MenusTreeDto()
                {
                    id = a.Code,
                    icon = a.Icon,
                    parent_id = a.ParentCode,
                    sort = a.Sort,
                    title = a.Name,
                    url = a.Url,
                    value = a.Id.ToString(),
                    @checked = auth_menus_id.Contains(a.Id),
                    disabled = false
                }
            ).ToListAsync();

            //返回多个根节点
            return await GetMenusTreeAsync(resutlMenus);
        }

        protected virtual async Task<List<T>> GetMenusTreeAsync<T>(List<T> menus) where T:MenusTree
        {
            var _type = typeof(T);
            //组装树
            async Task<IEnumerable<T>> GetChildNodesAsync(string parentNodeId)
            {
                var childs = menus.Where(a => a.parent_id == parentNodeId);
                foreach (var item in childs)
                {
                    if (_type == typeof(MenusNavDto))
                    {
                        (item as MenusNavDto).children = await GetChildNodesAsync(item.id) as IEnumerable<MenusNavDto>;
                    }
                    else
                    {
                        var data = item as MenusTreeDto;
                        data.data = await GetChildNodesAsync(item.id) as IEnumerable<MenusTreeDto>;
                        if (!data.data.Any())
                        {
                            //菜单加载完了，开始加载权限
                            data.data = await menuPermissionMapAppService.GetPermissionTreesAsync(new Guid(data.value), menuAndRoleMapTreeAllRetrieveInputDto.ProviderName, menuAndRoleMapTreeAllRetrieveInputDto.ProviderKey);
                        }
                    }
                }
                return childs;
            }

            //找到根节点
            var roots = menus.Where(a => a.parent_id == null || a.parent_id == "").OrderBy(a=>a.sort).ToList();
            foreach (var item in roots)
            {
                if (_type == typeof(MenusNavDto))
                {
                    (item as MenusNavDto).children = (await GetChildNodesAsync(item.id) as IEnumerable<MenusNavDto>).OrderBy(a=>a.sort);
                }
                else
                {
                    (item as MenusTreeDto).data = (await GetChildNodesAsync(item.id) as IEnumerable<MenusTreeDto>).OrderBy(a=>a.sort);
                }
            }
            return roots;
        }
    }
}
