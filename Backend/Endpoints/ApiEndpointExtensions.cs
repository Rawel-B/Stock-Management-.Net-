using Microsoft.AspNetCore.Authentication.JwtBearer;
using StockManagement.Models;
using StockManagement.Services;

namespace StockManagement.Endpoints;

public static class ApiEndpointExtensions
{
    public static IEndpointRouteBuilder MapStockManagementApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/auth/login", async (
            LoginRequest request,
            JwtTokenService tokenService) =>
        {
            var response = await tokenService.LoginAsync(request);

            if (response is null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(response);
        })
        .AllowAnonymous()
        .WithName("LoginWithJwt");

        var api = endpoints.MapGroup("/api")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        api.MapGet("/clients", async (ApiDataService apiDataService) =>
            await apiDataService.GetClientsAsync());

        api.MapPost("/clients", async (Client client, ApiDataService apiDataService) =>
        {
            var created = await apiDataService.CreateClientAsync(client);
            return Results.Created($"/api/clients/{created.Id}", created);
        });

        api.MapGet("/orders", async (ApiDataService apiDataService) =>
            await apiDataService.GetOrdersAsync());

        api.MapGet("/deliveries", async (ApiDataService apiDataService) =>
            await apiDataService.GetDeliveriesAsync());

        api.MapGet("/payments", async (ApiDataService apiDataService) =>
            await apiDataService.GetPaymentsAsync());

        return endpoints;
    }
}
