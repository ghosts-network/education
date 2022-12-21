using System;
using System.Net.Mime;
using GhostNetwork.Education.Api.Domain.FlashCards;
using GhostNetwork.Education.Api.Handlers.FlashCards;
using GhostNetwork.Education.Api.Integrations;
using GhostNetwork.Education.Api.Integrations.FlashCards;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace GhostNetwork.Education.Api;

public class Startup
{
    private const string DefaultDbName = "education";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("api", new OpenApiInfo
            {
                Title = "GhostNetwork.Education",
                Version = "1.0.0"
            });
        });

        services.AddScoped(_ =>
        {
            var connectionString = Configuration["MONGO_CONNECTION"];
            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);
            return new MongoDbContext(client.GetDatabase(mongoUrl.DatabaseName ?? DefaultDbName));
        });

        services.AddScoped<IFlashCardsProgressStorage, FlashCardsProgressStorage>(provider =>
            new FlashCardsProgressStorage(provider.GetRequiredService<MongoDbContext>()));

        services.AddSingleton<IFlashCardsCatalog>(FileBasedFlashCardsCatalog.Instance);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/api/swagger.json", "GhostNetwork.Education.Api v1");
            c.DisplayRequestDuration();
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints
                .MapGet("/flash-cards/sets/{setId}", GetSetByIdHandler.HandleAsync)
                .Produces<FlashCardsSet>(contentType: MediaTypeNames.Application.Json)
                .Produces(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
                .WithName("FlashCards_GetSetById")
                .WithTags("FlashCards");

            endpoints
                .MapGet("/flash-cards/sets", SearchSetsHandler.HandleAsync)
                .Produces<FlashCardsSet[]>(contentType: MediaTypeNames.Application.Json)
                .WithName("FlashCards_SearchSets")
                .WithTags("FlashCards");

            endpoints
                .MapPut("/flash-cards/sets/{setId}", SaveProgressHandler.HandleAsync)
                .Produces<FlashCardsSetUserProgress>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
                .ProducesValidationProblem(contentType: MediaTypeNames.Application.Json)
                .WithName("FlashCards_SaveProgress")
                .WithTags("FlashCards");
        });
    }
}