using Dapper;
using PetFamily.Core.Dapper;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Core;

public static class Inject
{
    public static void AddDapperTypeHandlers()
    {
        SqlMapper.AddTypeHandler(new JsonTypeHandler<List<RequisiteDto>>());
    }
}