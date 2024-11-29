using GF.Fussion.Data.Models.Demo;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using FastEndpoints;

namespace GF.Fussion.Web.Controllers.Demo;

public sealed class Add : Endpoint<MyRequestDto, MyResponseDto>
{
    public override void Configure ()
    {
        Post("/Demo/Add");

        Description(b => b
            .Accepts<MyRequestDto>("application/json+custom")
            .Produces<MyResponseDto>(200, "application/json+custom"));

        Summary(s => {
            s.Summary = "short summary goes here";
            s.Description = "long description goes here";
            s.ExampleRequest = new MyRequestDto() { Age = 18, FirstName = "Joe", LastName = "Doe" };
            s.ResponseExamples[200] = new MyResponseDto () { Age = 18, FullName = "Joe Doe", Id = 1 };
            s.Responses[200] = "ok response description goes here";
        });
    }

    public override async Task HandleAsync (MyRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.FirstName))
        {
            AddError(r => r.FirstName, "Firstname can not be empty.");
        }

        if (string.IsNullOrWhiteSpace(req.LastName))
        {
            AddError(r => r.LastName, "Lastname can not be empty.");
        }

        if (req.Age <= 0)
        {
            AddError(r => r.Age, "Age must be greatergreater than zero.");
        }

        ThrowIfAnyErrors();

        await SendOkAsync(new MyResponseDto()
        {
            FullName = $"{req.FirstName} {req.LastName}",
            Age = req.Age,
            Id = 1
        });
    }
}