namespace SubramanyaWebsite.Models;

public sealed class ServicePage
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Eyebrow { get; init; }
    public required string Summary { get; init; }
    public required string Image { get; init; }
    public required string ImageAlt { get; init; }
    public required string IntroTitle { get; init; }
    public required string Intro { get; init; }
    public required string[] Features { get; init; }
    public required string[] Benefits { get; init; }
    public required string Icon { get; init; }
    public required string MetaDescription { get; init; }
    public required ServiceSection[] Sections { get; init; }
    public required string[] Steps { get; init; }
    public string? ServiceNote { get; init; }
    public EquipmentCategory[] EquipmentCategories { get; init; } = [];
}

public sealed class ServiceSection
{
    public required string Title { get; init; }
    public required string Body { get; init; }
    public string[] Points { get; init; } = [];
}

public sealed class EquipmentCategory
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Image { get; init; }
    public required string ImageAlt { get; init; }
    public required string[] Examples { get; init; }
}
