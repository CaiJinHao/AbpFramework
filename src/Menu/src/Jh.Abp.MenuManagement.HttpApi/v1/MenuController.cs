﻿using Jh.Abp.MenuManagement.Menus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Jh.Abp.MenuManagement.v1
{
    [RemoteService]
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    public class MenuController: MenuManagementController
    {
        private readonly IMenuAppService menuAppService;
        public IDataFilter<ISoftDelete> dataFilter { get; set; }

        public MenuController(IMenuAppService _menuAppService)
        {
            menuAppService = _menuAppService;
        }

        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Menu> CreateAsync(MenuCreateInputDto input)
        {
            return await menuAppService.CreateAsync(input,true);
        }

        /// <summary>
        /// 创建多个实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("items")]
        [HttpPost]
        public async Task<Menu[]> CreateAsync(MenuCreateInputDto[] input)
        {
            return await menuAppService.CreateAsync(input);
        }

        /// <summary>
        /// 根据条件删除多条
        /// </summary>
        /// <param name="deleteInputDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Menu[]> DeleteAsync(MenuDeleteInputDto deleteInputDto)
        {
            return await menuAppService.DeleteAsync(deleteInputDto);
        }

        /// <summary>
        /// 根据主键删除多条
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [Route("keys")]
        [HttpDelete]
        public async Task<Menu[]> DeleteAsync([FromBody]Guid[] keys)
        {
            return await menuAppService.DeleteAsync(keys);
        }

        /// <summary>
        /// 根据条件查询(不分页)
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public async Task<ListResultDto<MenuDto>> GetEntitysAsync(MenuRetrieveInputDto inputDto)
        {
            return await menuAppService.GetEntitysAsync(inputDto);
        }

        /// <summary>
        /// 根据id更新部分
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<Menu> UpdatePortionAsync(Guid id, MenuUpdateInputDto inputDto)
        {
            return await menuAppService.UpdatePortionAsync(id, inputDto);
        }

        /// <summary>
        /// 根据ID更新全部
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<MenuDto> UpdateAsync(Guid id, MenuUpdateInputDto input)
        {
            return await menuAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<MenuDto>> GetListAsync(MenuRetrieveInputDto input)
        {
            using (dataFilter.Disable())
            {
                return await menuAppService.GetListAsync(input);
            }
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Menu> DeleteAsync(Guid id)
        {
            return await menuAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<MenuDto> GetAsync(Guid id)
        {
            return await menuAppService.GetAsync(id);
        }

        [HttpPatch]
        [Route("{id}/Deleted")]
        public virtual async Task UpdateDeletedAsync(Guid id, [FromBody] bool isDeleted)
        {
            using (dataFilter.Disable())
            {
                //await menuAppService.UpdateIsDeletedAsync(id, isDeleted);
                await menuAppService.UpdatePortionAsync(id, new MenuUpdateInputDto() {
                    UpdateEntityAction = (entity) =>{
                        entity.IsDeleted = isDeleted;
                    }
                });
            }
        }
    }
}
