namespace GF.Fussion.Data.Models.Demo;

public sealed record MyResponseDto
{
    public string? FullName { get; set; }
    public int Age { get; set; }
    public int Id { get; set; }
}