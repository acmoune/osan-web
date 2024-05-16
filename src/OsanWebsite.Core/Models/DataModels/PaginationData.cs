namespace OsanWebsite.Core.Models.DataModels;

public class PaginationData
{
    public Pagination pagination { get; set; } = default!;

    public class Pagination
    {
        public int page { get; set; } = default!;
        public int pageSize { get; set; } = default!;
        public int pageCount { get; set; } = default!;
        public int total { get; set; } = default!;
    }
}
