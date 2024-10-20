using System.Data;
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

namespace PetFamily.Species.Application.Queries.GetBreedsWithPagination;

public class GetBreedsWithPaginationQueryHandler
    : IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery>
{
    private readonly ILogger<GetBreedsWithPaginationQueryHandler> _logger;
    private readonly IValidator<GetBreedsWithPaginationQuery> _validator;
    private readonly ISqlConnectionFactory _factory;

    public GetBreedsWithPaginationQueryHandler(
        ILogger<GetBreedsWithPaginationQueryHandler> logger,
        IValidator<GetBreedsWithPaginationQuery> validator,
        ISqlConnectionFactory factory)
    {
        _logger = logger;
        _validator = validator;
        _factory = factory;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> Handle(
        GetBreedsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var connection = _factory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@SpeciesId", query.SpeciesId, DbType.Guid);

        var sqlTotal = "SELECT COUNT(1) FROM breeds WHERE species_id = @SpeciesId";

        var total = await connection.ExecuteScalarAsync<long>(sqlTotal, parameters);

        var sql = new StringBuilder("""
                                    SELECT 
                                        id,
                                        name
                                    FROM
                                        breeds
                                    WHERE 
                                        species_id = @SpeciesId
                                    """);

        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var breeds = await connection.QueryAsync<BreedDto>(sql.ToString(), parameters);

        return new PagedList<BreedDto>()
        {
            Items = breeds.ToList(),
            TotalCount = total,
            PageSize = query.PageSize,
            Page = query.Page
        };
    }
}