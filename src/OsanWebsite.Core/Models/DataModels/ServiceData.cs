namespace OsanWebsite.Core.Models.DataModels;

public class ServiceData
{
    public string id { get; set; } = default!;
    public ServiceAttributes attributes { get; set; } = default!;

    public Service ToService()
    {
        return new Service
        (
            Id: this.id,
            Name: this.attributes.Name,
            Description: this.attributes.Description,
            StartTime: this.attributes.StartTime,
            EndTime: this.attributes.EndTime
        );
    }
}

public class ServiceAttributes
{
    public string Name { get; set; } = default!;
    public string? Description {  get; set; } = default!;  
    public TimeOnly StartTime {  get; set; } = default!; 
    public TimeOnly EndTime { get; set; } = default!;
}

public class ServiceCollectionData
{
    public IEnumerable<ServiceData> data { get; set; } = default!;
}

public class ServiceItemData
{
    public ServiceData data { get; set; } = default!;
}

public class ServiceCollectionResponse
{
    public ServiceCollectionData services { get; set; } = default!;
}
