using System.Runtime.CompilerServices;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Dapper.SimpleSqlBuilder;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.DTOs.Species;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.GetPetById;

public class GetPetByIdQueryHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly ILogger<GetPetByIdQueryHandler> _logger;
    private readonly ISqlConnectionFactory _factory;

    public GetPetByIdQueryHandler(
        ILogger<GetPetByIdQueryHandler> logger,
        ISqlConnectionFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _factory.Create();

        var parameters = new DynamicParameters();
        parameters.AddDynamicParams(query);

        const string template = @$"
            select 
                pet.id as Id, 
                pet.name as Name, 
                pet.description as Description, 
                pet.help_status as Status, 
                pet.phone_number as Phone, 
                pet.volunteer_id as VolunteerId, 
                pet.requisites as Requisites, 
                pet.address_city as City, 
                pet.address_country as Country, 
                pet.address_flat as Flat, 
                pet.address_house as House, 
                pet.address_street as Street, 
                pet.birthday_date as BirthdayDate, 
                pet.color as Color, 
                pet.health_info as HealthInfo, 
                pet.height as Height, 
                pet.is_castrated as IsCastrated,
                pet.is_vaccinated as IsVaccinated, 
                pet.weight as Weight, 
                photo.id as PhotoId,
                photo.is_main as IsMain,
                photo.file_path as FilePath,
                pet.species_id as SpeciesId,
                species.name as SpeciesName,
                pet.breed_id as BreedId,
                pet.species_id as SpeciesId,
                breed.name as BreedName,
                trim(
                    concat(volunteer.first_name, ' ', volunteer.patronymic, ' ', volunteer.surname)) as FullName
            from
                pets pet
            left join 
                pet_photos photo
            on 
                photo.pet_id = pet.id
            inner join 
                volunteers volunteer
            on 
                pet.volunteer_id = volunteer.id
            inner join 
                breeds breed
            on 
                pet.breed_id = breed.id
            inner join 
                species
            on 
                pet.species_id = species.id
            ";

        var sql = FormSqlQuery(FormattableStringFactory.Create(template), parameters);

        var pets = new Dictionary<Guid, PetDto>();

        var petDto = await connection
            .QueryAsync<PetDto, string, AddressDto, PetDetailsDto, PetPhotoDto?, SpeciesDto, BreedDto, PetDto>(
                sql,
                (pet, requisitesJson, address, details, petPhoto, species, breed) =>
                {
                    if (!pets.TryGetValue(pet.Id, out var petDto))
                    {
                        var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(requisitesJson);

                        petDto = pet;
                        petDto.Address = address;
                        petDto.Requisites = requisites ?? [];
                        petDto.Details = details;
                        petDto.Photos ??= [];
                        petDto.Species = species;
                        petDto.Breed = breed;

                        pets.Add(petDto.Id, petDto);
                    }
                    
                    if (petPhoto is null) return petDto;
                    
                    if(petPhoto.IsMain)
                        petDto.Photos.Insert(0, petPhoto);
                    else
                        petDto.Photos.Add(petPhoto);

                    return petDto;
                },
                parameters,
                splitOn: "Requisites, City, BirthdayDate, PhotoId, SpeciesId, BreedId");

        var pet = pets.Values.FirstOrDefault();
        if (pet is null)
            return Errors.General.NotFound(query.PetId).ToErrorList();
        
        _logger.LogInformation("A pet {petId} was successfully received", query.PetId);

        return pet;
    }

    private string FormSqlQuery(FormattableString sql, DynamicParameters parameters)
    {
        return SimpleBuilder.Create(sql)
            .Append($"where pet.id = @PetId")
            .Sql;
    }
}