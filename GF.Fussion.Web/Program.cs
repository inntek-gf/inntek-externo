#region Imports.
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GF.Fussion.Web.Models.Sections;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using FastEndpoints.Swagger;
using System.Globalization;
using GF.Fussion.Data;
using System.Threading;
using FastEndpoints;
using System.Text;
using System;
#endregion

#region Normalization.
CultureInfo defaultCultureInfo = new("es-MX");
Thread.CurrentThread.CurrentUICulture = defaultCultureInfo;
Thread.CurrentThread.CurrentCulture = defaultCultureInfo;
Console.OutputEncoding = Encoding.UTF8;
#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Common Global Options.
IConfigurationSection commonConfigurationOptions = builder.Configuration
    .GetRequiredSection(CommonConfigurationSection.SectionName);

builder.Services
    .AddOptions<CommonConfigurationSection>()
    .Bind(commonConfigurationOptions)
    .ValidateOnStart();
#endregion

#region Plugins.
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Services.AddProblemDetails();
builder.Services.AddCors();
#endregion

#region Jwt Authentication.
builder.Services
   .AddAuthentication(
       o =>
       {
           o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       })
   .AddJwtBearer(
       o =>
       {
           o.TokenValidationParameters = new()
           {
               ValidateAudience = false,
               ValidateIssuer = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(commonConfigurationOptions.GetValue<string>("JWToken")!))
           };
       });
#endregion

#region Web Pages & API Controllers.
builder.Services.AddRazorPages();
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "Fussion EndPoints";
        };
    });
#endregion

#region Database & SQL Connection.
string connectionString = builder.Configuration.GetConnectionString("default") ?? throw new NullReferenceException();
builder.Services.AddTransient<SqlConnection>(provider => new SqlConnection(connectionString));
builder.Services.AddTransient<ISqlServerContext, SqlServerContext>();
#endregion

WebApplication app = builder.Build();

// Step 1 - Production Only Plugins.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseResponseCompression();
    app.UseHsts();
}

// Step 2 - Request Handles.
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});
app.UseStatusCodePages();

// Step 3 - Authorization.
app.UseAuthorization();
app.UseAuthentication();

// Step 4 - Allow Cross Origins.
app.UseCors(builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true));

// Step 5 - Running Application.
app.UseFastEndpoints(options =>
{
    options.Endpoints.RoutePrefix = "api";
})
    .UseSwaggerGen();
app.MapRazorPages();
app.Run();