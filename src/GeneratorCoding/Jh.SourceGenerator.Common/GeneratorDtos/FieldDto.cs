﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.SourceGenerator.Common.GeneratorDtos
{
    public class FieldDto
    {
        public bool IsRequired { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// domain 必须带Description特性
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 字段的类型
        /// </summary>
        public string Type { get; set; }
    }
}
