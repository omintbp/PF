using System.Data;
using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionByRelationId;

public class GetDiscussionByRelationIdQueryHandler
    : IQueryHandler<DiscussionDto, GetDiscussionByRelationIdQuery>
{
    private readonly IValidator<GetDiscussionByRelationIdQuery> _validator;
    private readonly ISqlConnectionFactory _factory;

    public GetDiscussionByRelationIdQueryHandler(
        IValidator<GetDiscussionByRelationIdQuery> validator,
        ISqlConnectionFactory factory)
    {
        _validator = validator;
        _factory = factory;
    }

    public async Task<Result<DiscussionDto, ErrorList>> Handle(
        GetDiscussionByRelationIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var connection = _factory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@RelationId", query.RelationId, DbType.Guid);

        var sql = new StringBuilder($"""
                                     select 
                                       d.id as DiscussionId,
                                       d.relation_id RelationId,
                                       d.users DiscussionUsers,
                                       m.id as MessageId,
                                       m.is_edited IsEdited,
                                       m.created_at CreatedAt,
                                       m.text as Text,
                                       u.id as UserId,
                                       u.user_name as Username,
                                       u.first_name as FirstName,
                                       u.patronymic as Patronymic,
                                       u.surname as Surname,
                                       u.email as UserEmail
                                     from 
                                         discussions d
                                     left join 
                                         messages m on m.discussion_id = d.id
                                     left join 
                                         users u on u.id = m.user_id 
                                     where 
                                         d.relation_id = @RelationId
                                     """);

        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var discussions = new Dictionary<Guid, DiscussionDto>();

        var result = await connection.QueryAsync<DiscussionDto, MessageDto?, DiscussionDto>(
            sql.ToString(),
            (discussionDto, messageDto) =>
            {
                if (!discussions.TryGetValue(discussionDto.DiscussionId, out var discussion))
                {
                    discussion = discussionDto;
                    discussions.Add(discussionDto.DiscussionId, discussion);
                }

                if (messageDto is not null)
                {
                    discussion.Messages.Add(messageDto);
                    discussion.TotalMessagesCount++;
                }
                
                return discussion;
            },
            parameters,
            splitOn: "MessageId");

        var discussion = result.FirstOrDefault();

        if (discussion == null)
            return Errors.General.NotFound(query.RelationId).ToErrorList();

        return discussion;
    }
}