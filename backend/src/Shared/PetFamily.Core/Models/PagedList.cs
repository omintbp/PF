namespace PetFamily.Core.Models;

public class PagedList<T>
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public IReadOnlyList<T> Items { get; init; } = [];

    public long TotalCount { get; init; }

    public bool HasNextPage => Page * PageSize < TotalCount;

    public bool HasPreviousPage => Page > 1;
}