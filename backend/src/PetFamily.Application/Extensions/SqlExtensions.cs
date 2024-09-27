using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;

namespace PetFamily.Application.Extensions;

public static class SqlExtensions
{
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        parameters.Add("@PageSize", pageSize);
        parameters.Add("@Offset", (page - 1) * pageSize);

        sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
    }
}