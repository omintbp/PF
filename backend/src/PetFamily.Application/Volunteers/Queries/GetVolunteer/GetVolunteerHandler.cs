using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteer;

public class GetVolunteerHandler : IQueryHandler<VolunteerDto, GetVolunteerQuery>
{
    private readonly ILogger<GetVolunteerHandler> _logger;
    private readonly ISqlConnectionFactory _factory;

    public GetVolunteerHandler(ILogger<GetVolunteerHandler> logger, ISqlConnectionFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(GetVolunteerQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _factory.Create();

        var parameters = new DynamicParameters();

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
                                        volunteers v
                                    WHERE
                                        id = @id
                                    """);

        parameters.Add("@id", query.Id);

        var volunteers = await connection.QueryVolunteersAsync(sql.ToString(), parameters);
        var volunteer = volunteers.FirstOrDefault();

        if (volunteer is null)
            return Errors.General.NotFound().ToErrorList();
        
        _logger.LogInformation("Got volunteer with id = {Id}", query.Id);

        return volunteer;
    }
}