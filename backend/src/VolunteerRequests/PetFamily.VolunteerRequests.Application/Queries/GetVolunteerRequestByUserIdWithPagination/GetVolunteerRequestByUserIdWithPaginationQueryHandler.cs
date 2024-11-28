using System.Data;
using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Contracts.DTOs;

namespace PetFamily.VolunteerRequests.Application.Queries.GetVolunteerRequestByUserIdWithPagination;

public class GetVolunteerRequestByUserIdWithPaginationQueryHandler
    : IQueryHandler<PagedList<VolunteerRequestDto>, GetVolunteerRequestByUserIdWithPaginationQuery>
{
    private readonly IValidator<GetVolunteerRequestByUserIdWithPaginationQuery> _validator;
    private readonly ISqlConnectionFactory _factory;

    public GetVolunteerRequestByUserIdWithPaginationQueryHandler(
        IValidator<GetVolunteerRequestByUserIdWithPaginationQuery> validator,
        ISqlConnectionFactory factory)
    {
        _validator = validator;
        _factory = factory;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> Handle(
        GetVolunteerRequestByUserIdWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var connection = _factory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@UserId", query.UserId, DbType.Guid);
        parameters.Add("@Status", query.Status, DbType.String);

        var totalCount = await connection.ExecuteScalarAsync<long>(
            $"select count(1) from volunteer_requests where user_id = @UserId",
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
                                         user_id = @UserId
                                     """);

        sql.AppendLineIf(query.Status != null, " and status = @Status");

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