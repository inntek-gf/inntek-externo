using FastEndpoints;
using GF.Fussion.Data.Models.Demo;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace GF.Fussion.Web.Controllers.Demo;

public sealed class GetById : EndpointWithoutRequest<MyResponseDto>
{
    public override void Configure ()
    {
        Get("/Demo/GetById/{demoId}");

        Description(b => b
            .Produces<MyResponseDto>(200, "application/json+custom"));

        Summary(s =>
        {
            s.Params["demoId"] = "Id de prueba";
        });
    }

    public override async Task HandleAsync (CancellationToken ct)
    {
        int demoId = Route<int>("demoId");

        if (demoId <= 0)
        {
            ThrowError("Demo Id must be greater than zero."); 
        }

        MyResponseDto res = new ()
        {
            FullName = "FullName",
            Id = demoId,
            Age = 20
        };

        await SendOkAsync(res);
    }
}
