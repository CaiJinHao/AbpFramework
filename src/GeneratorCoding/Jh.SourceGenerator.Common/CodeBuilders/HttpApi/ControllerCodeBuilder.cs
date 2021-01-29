﻿using Jh.SourceGenerator.Common.GeneratorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.SourceGenerator.Common.CodeBuilders
{
    public class ControllerCodeBuilder : CodeBuilderAbs
    {
        public ControllerCodeBuilder(TableDto tableDto) : base(tableDto)
        {
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(@"using Jh.Abp.Application.Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;");
            builder.AppendLine($"namespace {table.Namespace}");
            builder.AppendLine("{");
            {
                builder.AppendLine("\t[RemoteService]");
                builder.AppendLine("\t[Route(\"api/v{apiVersion:apiVersion}/[controller]\")]");
                builder.AppendLine($"\tpublic class {table.Name}Controller : {table.ControllerBase}");
                builder.AppendLine("\t{");
                {
                    builder.AppendLine($"\t\tprivate readonly I{table.Name}AppService {table.Name}AppService;");
                    builder.AppendLine("\t\tpublic IDataFilter<ISoftDelete> dataFilter { get; set; }");
                    builder.AppendLine($"\t\tpublic {table.Name}Controller(I{table.Name}AppService _{table.Name}AppService)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\t{table.Name}AppService = _{table.Name}AppService;");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpPost]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}> CreateAsync({table.Name}CreateInputDto input)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.CreateAsync(input,true);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[Route(\"items\")]");
                    builder.AppendLine("\t\t[HttpPost]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}[]> CreateAsync({table.Name}CreateInputDto[] input)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.CreateAsync(input);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpDelete]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}[]> DeleteAsync({table.Name}DeleteInputDto deleteInputDto)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.DeleteAsync(deleteInputDto);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[Route(\"keys\")]");
                    builder.AppendLine("\t\t[HttpDelete]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}[]> DeleteAsync([FromBody]{table.KeyType}[] keys)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.DeleteAsync(keys);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[Route(\"all\")]");
                    builder.AppendLine("\t\t[HttpGet]");
                    builder.AppendLine($"\t\tpublic async Task<ListResultDto<{table.Name}Dto>> GetEntitysAsync({table.Name}RetrieveInputDto inputDto)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.GetEntitysAsync(inputDto);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpPatch(\"{id}\")]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}> UpdatePortionAsync({table.KeyType} id, {table.Name}UpdateInputDto inputDto)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.UpdatePortionAsync(id, inputDto);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpPut(\"{id}\")]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}Dto> UpdateAsync({table.KeyType} id, {table.Name}UpdateInputDto input)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.UpdateAsync(id, input);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpGet]");
                    builder.AppendLine($"\t\tpublic async Task<PagedResultDto<{table.Name}Dto>> GetListAsync({table.Name}RetrieveInputDto input)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine("\t\t\tusing (dataFilter.Disable())");
                        builder.AppendLine("\t\t\t{");
                        builder.AppendLine($"\t\t\t\treturn await {table.Name}AppService.GetListAsync(input);");
                        builder.AppendLine("\t\t\t}");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpDelete(\"{id}\")]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}> DeleteAsync({table.KeyType} id)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.DeleteAsync(id);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpGet(\"{id}\")]");
                    builder.AppendLine($"\t\tpublic async Task<{table.Name}Dto> GetAsync({table.KeyType} id)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine($"\t\t\treturn await {table.Name}AppService.GetAsync(id);");
                        builder.AppendLine("}");
                    }

                    builder.AppendLine("\t\t[HttpPatch]");
                    builder.AppendLine("\t\t[Route(\"{id}/Deleted\")]");
                    builder.AppendLine($"\t\tpublic async Task UpdateDeletedAsync({table.KeyType} id, [FromBody] bool isDeleted)");
                    {
                        builder.AppendLine("{");
                        builder.AppendLine("\t\t\tusing (dataFilter.Disable())");
                        builder.AppendLine("\t\t\t{");
                        builder.AppendLine($"\t\t\t\tawait {table.Name}AppService.UpdatePortionAsync(id, new {table.Name}UpdateInputDto()");
                        builder.AppendLine("\t\t\t\t{");
                        builder.AppendLine($"\t\t\t\t\tMethodInput = new MethodDto<{table.Name}>()");
                        builder.AppendLine("\t\t\t\t\t{");
                        builder.AppendLine("\t\t\t\t\t\tUpdateEntityAction = (entity) => entity.IsDeleted = isDeleted");
                        builder.AppendLine("\t\t\t\t\t}");
                        builder.AppendLine("\t\t\t\t}");
                        builder.AppendLine("\t\t\t}");
                        builder.AppendLine("}");
                    }
                }
                builder.AppendLine("\t}");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
