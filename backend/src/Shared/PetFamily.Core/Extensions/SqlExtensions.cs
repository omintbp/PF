using System.Data;
using System.Text;
using Dapper;
using Dapper.SimpleSqlBuilder;
using PetFamily.Core.Models;

namespace PetFamily.Core.Extensions;

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

    public static StringBuilder ApplySorting(
        this StringBuilder sqlBuilder,
        string? sortBy,
        string? sortDirection)
    {
        List<string> directions = ["asc", "desc"];

        var isValidDirection = sortDirection != null &&
                               directions.Contains(sortDirection.ToLower());

        if (string.IsNullOrWhiteSpace(sortBy) || isValidDirection == false)
        {
            return sqlBuilder;
        }

        return sqlBuilder.AppendLine($" order by {sortBy} {sortDirection}");
    }
}