using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;
using PetFamily.Application.SharedDTOs;

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

    public static async Task<List<VolunteerDto>> QueryVolunteersAsync(
        this IDbConnection connection, string sql, DynamicParameters parameters)
    {
        var result = await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
            sql,
            (volunteer, requisitesJson, socialNetworksJson) =>
            {
                var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(requisitesJson);
                var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(socialNetworksJson);

                volunteer.Requisites = requisites ?? [];
                volunteer.SocialNetworks = socialNetworks ?? [];

                return volunteer;
            },
            splitOn: "requisites,social_networks",
            param: parameters);

        return result.ToList();
    }
}