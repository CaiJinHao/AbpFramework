﻿using Jh.SourceGenerator.Common.GeneratorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.SourceGenerator.Common.CodeBuilders
{
    public class CreateInputDtoCodeBuilder : CodeBuilderAbs
    {
        public CreateInputDtoCodeBuilder(TableDto tableDto, string filePath) : base(tableDto, filePath)
        {
            this.FileName = $"{table.Name}CreateInputDto";
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(@"using System;
using System.ComponentModel.DataAnnotations;
using Jh.Abp.Application.Contracts.Dtos;
using Jh.Abp.Application.Contracts.Extensions;
using Volo.Abp.ObjectExtending;");
            builder.AppendLine($"namespace {table.Namespace}");
            builder.AppendLine("{");
            {
                builder.AppendLine($"\tpublic class {FileName}: ");
                if (table.IsConcurrencyStamp)
                {
                    builder.AppendLine($"ExtensibleObject,");
                }
                builder.AppendLine($"IMethodDto<{table.Name}>");

                builder.AppendLine("\t{");
                {
                    foreach (var _field in table.FieldsCreateOrUpdateInput)
                    {
                        builder.AppendLine($"\t\t/// <summary>");
                        builder.AppendLine($"\t\t/// {_field.Description}");
                        builder.AppendLine($"\t\t/// <summary>");
                        if (_field.IsRequired)
                        {
                            builder.AppendLine($"\t\t[Required]");
                        }
                        var nullable = _field.IsNullable ? "?" : "";//可空类型
                        builder.AppendLine($"\t\tpublic {_field.Type}{nullable} {_field.Name} " + "{ get; set; }");
                    }
                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine("\t\t/// 方法参数回调");
                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine($"\t\tpublic MethodDto<{table.Name}> MethodInput " + "{ get; set; }");
                }
                builder.AppendLine("\t}");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
