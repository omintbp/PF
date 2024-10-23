using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.DTOs.Species;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationQueryHandler
    : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly ILogger<GetSpeciesWithPaginationQueryHandler> _logger;
    private readonly IValidator<GetSpeciesWithPaginationQuery> _validator;
    private readonly ISqlConnectionFactory _factory;

    public GetSpeciesWithPaginationQueryHandler(
        ILogger<GetSpeciesWithPaginationQueryHandler> logger,
        IValidator<GetSpeciesWithPaginationQuery> validator,
        ISqlConnectionFactory factory)
    {
        _logger = logger;
        _validator = validator;
        _factory = factory;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
        var connection = _factory.Create();

        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>("SELECT count(1) FROM species ");

        var sql = new StringBuilder("""
                                        SELECT
                                            id,
                                            name
                                        FROM
                                            species
                                    """);
        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var result = await connection.QueryAsync<SpeciesDto>(sql.ToString(), parameters);

        return new PagedList<SpeciesDto>()
        {
            Items = result.ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}