using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.Abp.MenuManagement
{
    public class MyUsers
    {
        /// <summary>
        /// 用户所属平台类型
        /// </summary>
        public int PlatformType { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction { get; set; }
    }
}
