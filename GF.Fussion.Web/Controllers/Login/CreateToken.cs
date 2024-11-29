using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using GF.Fussion.Data.Models.Login;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using FastEndpoints;
using System.Text;
using System;
using Microsoft.Extensions.Options;
using GF.Fussion.Web.Models.Sections;

namespace GF.Fussion.Web.Controllers.Login;

/// <summary>
/// <c>ApiEndpoint</c> Generacion de token para inicio de session en la aplicacion. 
/// <c>Author:</c> Fco. Eduardo. <c>Date:</c> 19/11/2024.
/// </summary>
/// <remarks>
/// <c>POST: /api/Login/CreateToken</c>
/// </remarks>
public sealed class CreateToken(IOptions<CommonConfigurationSection> commonConfiguration) : Endpoint<TokenRequestDto>
{
    private readonly CommonConfigurationSection _commonConfiguration = commonConfiguration.Value;

    public sealed override void Configure ()
    {
        Post("/Login/CreateToken");
        AllowAnonymous();

        Description(b => b
            .Accepts<TokenRequestDto>("application/json+custom")
            .Produces<string>(200, "plain/text"));

        Summary(s => {
            s.Summary = "Generate session token.";
            s.Description = "Authorize user credentials to generate session bearer token.";
            s.ExampleRequest = new TokenRequestDto() { Email = "user@domain.com", Secret = "secret" };
            s.ResponseExamples[200] = "Token";
            s.Responses[200] = "Bearer token";
        });
    }

    private SecurityTokenDescriptor GetSecurityTokenDescriptor (TokenRequestDto request)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            SigningCredentials = GetSigningCredentials(),
            Expires = DateTime.UtcNow.AddHours(1),
            Audience = "https://localhost:7141",
            Issuer = "https://localhost:7141",
            
            // TODO: Get user session information from database.
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Sid, "001"),
                new Claim(ClaimTypes.Name, "user name"),
                new Claim(ClaimTypes.Email, request.Email),
            ])
        };

        return tokenDescriptor;
    }
    private SigningCredentials GetSigningCredentials ()
    {
        byte[] key = Encoding.UTF8
            .GetBytes(_commonConfiguration.JWToken);

        return new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
    }
    private void ValidateRequest (TokenRequestDto request)
    {
        if (request is null)
        {
            ThrowError("Request information is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            AddError(r => r.Email, "Email information is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Secret))
        {
            AddError(r => r.Secret, "Secret information is required.");
        }

        ThrowIfAnyErrors();
    }

    public sealed override async Task HandleAsync (TokenRequestDto request, CancellationToken ct)
    {
        ValidateRequest(request);

        JwtSecurityTokenHandler tokenHandler = new ();
        SecurityTokenDescriptor tokenDescriptor = GetSecurityTokenDescriptor(request);
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        string token = tokenHandler.WriteToken(securityToken);

        await SendOkAsync(token, ct);
    }
}