namespace EternalKids.Domain.Entities;

public class PackageDefinition
{
    public Guid Id { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FeaturesJson { get; set; } = "[]";
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset UpdatedAtUtc { get; set; }
}
