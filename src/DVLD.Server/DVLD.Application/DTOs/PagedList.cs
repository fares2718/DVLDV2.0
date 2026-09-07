namespace DVLD.Application.DTOs;

public sealed class PagedList<T>(
    IReadOnlyList<T> items,
    int totalCount,
    int pageNumber,
    int pageSize)
{
    public IReadOnlyList<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
    public int TotalPages { get; } = (int)Math.Ceiling(
        totalCount / (double)pageSize);

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}