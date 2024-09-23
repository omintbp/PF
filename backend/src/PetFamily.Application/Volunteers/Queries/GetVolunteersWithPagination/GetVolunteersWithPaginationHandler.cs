using System.Data;
using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler
    : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;
    private readonly ISqlConnectionFactory _factory;

    public GetVolunteersWithPaginationHandler(
        ILogger<GetVolunteersWithPaginationHandler> logger,
        ISqlConnectionFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _factory.Create();

        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>(
            "  SELECT COUNT(1) FROM volunteers;");

        var sql = new StringBuilder("""
                                    SELECT 
                                        id, 
                                        description, 
                                        email, 
                                        experience, 
                                        first_name, 
                                        patronymic, 
                                        surname, 
                                        phone_number, 
                                        requisites, 
                                        social_networks 
                                    FROM 
                                        volunteers 
                                    """);

        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var volunteers = await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
            sql.ToString(),
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

        var pagedList = new PagedList<VolunteerDto>()
        {
            Items = volunteers.ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };

        return pagedList;
    }
}