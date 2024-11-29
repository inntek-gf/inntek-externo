namespace GF.Fussion.Data.Models.Login;

public sealed record TokenRequestDto
{
    public string Email { get; set; }
    public string Secret { get; set; }
}
