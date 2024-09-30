using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Dapper.SimpleSqlBuilder;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Species;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersHandlers.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryHandler
    : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ILogger<GetFilteredPetsWithPaginationQueryHandler> _logger;
    private readonly ISqlConnectionFactory _factory;

    public GetFilteredPetsWithPaginationQueryHandler(
        ILogger<GetFilteredPetsWithPaginationQueryHandler> logger,
        ISqlConnectionFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var sortDirection = query.SortPetDirection == SortDirection.Ascending ? "asc" : "desc";
        var sortBy = GetSortColumnName(query.SortPetBy);

        var connection = _factory.Create();

        var totalCount = await connection.ExecuteScalarAsync<long>(
            "  SELECT COUNT(1) FROM pets;");

        var parameters = new DynamicParameters();
        parameters.AddDynamicParams(query);
        parameters.Add("@SortOptions", $"{sortBy} {sortDirection}", DbType.String);

        var pets = new Dictionary<Guid, PetDto>();

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

        var sql = FormSqlQuery(FormattableStringFactory.Create(template), query, parameters);

        var _ = await connection
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

        _logger.LogInformation("Got {petsCount} pets from {totalCount}", pets.Values.Count, totalCount);
        
        return new PagedList<PetDto>()
        {
            Items = pets.Values.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page
        };
    }

    private string FormSqlQuery(
        FormattableString sql,
        GetFilteredPetsWithPaginationQuery query,
        DynamicParameters parameters)
    {
        return SimpleBuilder.Create(sql)
            .Append($"where 1 = 1")
            .AppendNewLine(!string.IsNullOrEmpty(query.Country), @$" and pet.address_country = @Country")
            .AppendNewLine(!string.IsNullOrEmpty(query.City), @$" and pet.address_city = @City")
            .AppendNewLine(!string.IsNullOrEmpty(query.House), @$" and pet.address_house = @House")
            .AppendNewLine(!string.IsNullOrEmpty(query.Color), @$" and pet.color = @Color")
            .AppendNewLine(!string.IsNullOrEmpty(query.Street), @$" and pet.address_street = @Street")
            .AppendNewLine(!string.IsNullOrEmpty(query.BreedName), @$" and breed.name = @BreedName")
            .AppendNewLine(!string.IsNullOrEmpty(query.PetName), @$" and pet.name = @PetName")
            .AppendNewLine(!string.IsNullOrEmpty(query.PhoneNumber), @$" and pet.phone_number = @Phone")
            .AppendNewLine(!string.IsNullOrEmpty(query.SpeciesName), @$" and species.name = @SpeciesName")
            .AppendNewLine(query.MinHeight is not null, @$" and pet.height >= @MinHeight")
            .AppendNewLine(query.MaxHeight is not null, @$" and pet.height <= @MaxHeight")
            .AppendNewLine(query.MinWeight is not null, @$" and pet.weight >= @MinWeight")
            .AppendNewLine(query.MaxWeight is not null, @$" and pet.weight <= @MaxWeight")
            .AppendNewLine(query.VolunteerId is not null, @$" and pet.volunteer_id = @VolunteerId")
            .AppendNewLine(query.MaxAge is not null,
                @$" and DATE_PART('YEAR',AGE(CURRENT_DATE, pet.birthday_date)) <= @MaxAge")
            .AppendNewLine(query.MinAge is not null,
                @$" and DATE_PART('YEAR',AGE(CURRENT_DATE, pet.birthday_date)) >= @MinAge")
            .AppendNewLine($" order by @SortOptions")
            .ApplyPagination(parameters, query.Page, query.PageSize).Sql;
    }

    private string GetSortColumnName(SortPetBy? sortPetBy) =>
        sortPetBy switch
        {
            SortPetBy.Age => "BirthdayDate",
            SortPetBy.Name => "Name",
            SortPetBy.Breed => "BreedName",
            SortPetBy.Species => "SpeciesName",
            SortPetBy.Color => "Color",
            SortPetBy.Volunteer => "FullName",
            _ => "Id"
        };
}