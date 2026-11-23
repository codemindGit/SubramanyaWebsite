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
}
