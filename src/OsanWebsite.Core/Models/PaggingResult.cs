namespace OsanWebsite.Core.Models;

public class PaggingResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems {  get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    
    public PaggingResult(IEnumerable<T> items, int totalItems, int pageSize, int totalPages, int currentPage)
    {
        Items = items;
        TotalItems = totalItems;
        PageSize = pageSize;
        TotalPages = totalPages;
        CurrentPage = currentPage;
    }
}
