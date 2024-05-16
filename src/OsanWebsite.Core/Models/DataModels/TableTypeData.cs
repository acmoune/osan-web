namespace OsanWebsite.Core.Models.DataModels;

public class TableTypeData
{
    public string id { get; set; } = default!;
    public TableTypeAttributes attributes { get; set; } = default!;

    public TableType ToTableType()
    {
        return new TableType
        (
            Id: this.id,
            Name: this.attributes.Name,
            NumberOfPlaces: this.attributes.NumberOfPlaces
        );
    }
}

public class TableTypeAttributes
{
    public string Name { get; set; } = default!;
    public int NumberOfPlaces { get; set; }
}

public class TableTypeCollectionData
{
    public IEnumerable<TableTypeData> data { get; set; } = default!;
}

public class TableTypeItemData
{
    public TableTypeData data { get; set; } = default!;
}

public class TableTypeCollectionResponse
{
    public TableTypeCollectionData tableTypes { get; set; } = default!;
}
