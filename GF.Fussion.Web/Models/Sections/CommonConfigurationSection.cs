namespace GF.Fussion.Web.Models.Sections;

public sealed record CommonConfigurationSection
{
    public const string SectionName = "CommonConfiguration";
    public required string Title { get; init; }
    public required string ShortTitle { get; init; }
    public string? Description { get; init; }
}