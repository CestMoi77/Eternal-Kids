namespace EternalKids.Domain.Entities;

public class SiteContentBlock
{
    public Guid Id { get; set; }
    public string PageKey { get; set; } = string.Empty;
    public string SectionKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAtUtc { get; set; }
}
