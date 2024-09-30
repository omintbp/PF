using System.Text.Json.Serialization;

namespace PetFamily.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Ascending,
    Descending
}