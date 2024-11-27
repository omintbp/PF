using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using Dapper.SimpleSqlBuilder;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Queries.GetUnclaimedVolunteerRequests;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Queries.GetUnclaimedVolunteerRequestsWithPagination;

public class GetUnclaimedVolunteerRequestsWithPaginationQueryHandler
    : IQueryHandler<PagedList<VolunteerRequestDto>, GetUnclaimedVolunteerRequestsWithPaginationQuery>
{
    private readonly IValidator<GetUnclaimedVolunteerRequestsWithPaginationQuery> _validator;
    private readonly ISqlConnectionFactory _factory;

    public GetUnclaimedVolunteerRequestsWithPaginationQueryHandler(
        IValidator<GetUnclaimedVolunteerRequestsWithPaginationQuery> validator,
        ISqlConnectionFactory factory)
    {
        _validator = validator;
        _factory = factory;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> Handle(
        GetUnclaimedVolunteerRequestsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var connection = _factory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@Status", VolunteerRequestStatus.Submitted.Status, DbType.String);

        var totalCount = await connection.ExecuteScalarAsync<long>(
            $"select count(1) from volunteer_requests where status = @Status",
            parameters);

        var sql = new StringBuilder($"""
                                     select 
                                         id as Id, 
                                         admin_id as AdminId, 
                                         user_id as UserId,
                                         discussion_id as DiscussionId,
                                         experience as Experience,
                                         requisites as Requisites, 
                                         created_at as CreatedAt, 
                                         status as Status, 
                                         rejection_comment as RejectionComment
                                     from 
                                         volunteer_requests
                                     where 
                                         status = @Status
                                     """);

        sql.ApplySorting(query.SortBy, query.SortDirection);

        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var result = await connection.QueryAsync<VolunteerRequestDto>(
            sql.ToString(),
            parameters);

        return new PagedList<VolunteerRequestDto>()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            Items = result.ToList()
        };
    }
}