using GF.Fussion.Data.Models.Demo;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using FastEndpoints;

namespace GF.Fussion.Web.Controllers.Demo;

public sealed class GetAll : EndpointWithoutRequest<IEnumerable<MyResponseDto>>
{
    public override void Configure ()
    {
        Get("/Demo/GetAll");
        AllowAnonymous();
    }

    public override async Task HandleAsync (CancellationToken ct)
    {
        string? userName = User.FindFirstValue(ClaimTypes.Name) ?? "Nombre del usuario";

        MyResponseDto[] res = [
            
            new MyResponseDto()
            {
                FullName = userName,
                Age = 20,
                Id = 1
            },
            new MyResponseDto()
            {
                FullName = userName,
                Age = 20,
                Id = 2
            },
            new MyResponseDto()
            {
                FullName = userName,
                Age = 20,
                Id = 3
            }
        ];

        await SendOkAsync(res);
    }
}