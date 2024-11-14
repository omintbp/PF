using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Accounts;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<UserDto, GetUserByIdQuery>
{
    private readonly IAccountReadDbContext _context;

    public GetUserByIdQueryHandler(IAccountReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.ParticipantAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if(user is null)
            return Errors.General.NotFound(query.UserId).ToErrorList();
        
        return user;
    }
}