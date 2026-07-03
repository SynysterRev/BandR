namespace BandR.Common;

public class AnnouncementQueryFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? Search { get; set; }
    
    public string? City { get; set; }
    public List<string>? Instruments { get; set; } = [];
    public List<string>? Styles { get; set; } = [];
    public List<string>? Tags { get; set; } = [];
}