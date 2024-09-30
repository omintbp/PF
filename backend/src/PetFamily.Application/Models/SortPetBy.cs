using System.Text.Json.Serialization;

namespace PetFamily.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortPetBy
{
    Name = 0,
    Age = 1,
    Species = 2,
    Color = 3,
    Breed = 4,
    Volunteer = 5
}