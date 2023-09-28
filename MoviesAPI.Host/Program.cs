using Microsoft.Extensions.DependencyInjection;
using MoviesAPI.Domain.Repositories;
using MoviesAPI.Infrastructure.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Application.Common.Mapping.MoviesAPI.Application.Mapping;
using MoviesAPI.Application.Viewers.Queries;
using MoviesAPI.Infrastructure.Repositories;
using MoviesAPI.Application.Common.Interfaces;
using MoviesAPI.Infrastructure.Services;
using System.Net.Http.Headers;
using MoviesAPI.Application.Managers.Queries;
using FluentValidation;
using MoviesAPI.Application.Managers.QueryValidators;
using MediatR;
using MoviesAPI.Application.Abstractions;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");

var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());
builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), keyVaultClientId.Value.ToString(), keyVaultClientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());
var keyVaultClient = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetIntelligentBillboardQuery).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(GetIntelligentBillboardQueryValidator).Assembly); ;
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(keyVaultClient.GetSecret("ProdConnection").Value.Value.ToString());
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
{
    var apiUrl = builder.Configuration["TMDBApiStrings:TMDBApiUrl"];
    var bearerToken = keyVaultClient.GetSecret("TMDBBearerToken").Value.Value.ToString();

    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
});

builder.Services.AddScoped<IManagersRepository, ManagersRepository>();
builder.Services.AddScoped<IViewersRepository, ViewersRepository>();
builder.Services.AddScoped<ITMDBMovieRepository, TMDBMovieRepository>();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger/");
        return Task.CompletedTask;
    });
});

app.Run();
