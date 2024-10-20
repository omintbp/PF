using System.Text.Json.Serialization;

namespace PetFamily.Core.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Ascending,
    Descending
}