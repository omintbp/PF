using System.Text;
using Dapper;
using Dapper.SimpleSqlBuilder;

namespace PetFamily.Application.Extensions;

public static class SqlExtensions
{
    public static StringBuilder ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        parameters.Add("@PageSize", pageSize);
        parameters.Add("@Offset", (page - 1) * pageSize);

        sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");

        return sqlBuilder;
    }
    
    public static Builder ApplyPagination(
        this Builder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        parameters.Add("@PageSize", pageSize);
        parameters.Add("@Offset", (page - 1) * pageSize);

        return sqlBuilder.AppendNewLine($" LIMIT @PageSize OFFSET @Offset");
    }
}