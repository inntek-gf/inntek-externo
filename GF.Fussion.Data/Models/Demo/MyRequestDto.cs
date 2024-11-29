namespace GF.Fussion.Data.Models.Demo;

public sealed record MyRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
}