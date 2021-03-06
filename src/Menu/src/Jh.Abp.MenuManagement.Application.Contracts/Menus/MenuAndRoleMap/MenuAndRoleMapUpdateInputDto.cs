﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.MultiTenancy;

namespace Jh.Abp.MenuManagement
{
    public class MenuAndRoleMapUpdateInputDto: IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        [Required]
        public Guid MenuId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 权限名称列表
        /// </summary>
        public string[] PermissionNames { get; set; }
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
    }
}
