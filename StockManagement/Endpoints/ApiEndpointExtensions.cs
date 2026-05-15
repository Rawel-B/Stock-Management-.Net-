using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockManagement.Data;
using StockManagement.Models;

namespace StockManagement.Endpoints;

public static class ApiEndpointExtensions
{
    public static IEndpointRouteBuilder MapStockManagementApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/auth/login", async (
            LoginRequest request,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Unauthorized();
            }

            var expiresAt = DateTime.UtcNow.AddHours(8);
            var jwt = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? request.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Email ?? request.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return Results.Ok(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, user.Email ?? request.Email));
        })
        .AllowAnonymous()
        .WithName("LoginWithJwt");

        var api = endpoints.MapGroup("/api")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        api.MapGet("/clients", async (ApplicationDbContext dbContext) =>
            await dbContext.Clients.OrderBy(client => client.Name).ToListAsync());

        api.MapPost("/clients", async (Client client, ApplicationDbContext dbContext) =>
        {
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/clients/{client.Id}", client);
        });

        api.MapGet("/orders", async (ApplicationDbContext dbContext) =>
            await dbContext.Orders
                .Include(order => order.Client)
                .Include(order => order.Lines)
                .ThenInclude(line => line.Product)
                .OrderByDescending(order => order.Date)
                .ToListAsync());

        api.MapGet("/deliveries", async (ApplicationDbContext dbContext) =>
            await dbContext.Deliveries
                .Include(delivery => delivery.Order)
                .ThenInclude(order => order!.Client)
                .Include(delivery => delivery.Transporter)
                .OrderByDescending(delivery => delivery.DeliveryDate)
                .ToListAsync());

        api.MapGet("/payments", async (ApplicationDbContext dbContext) =>
            await dbContext.Payments
                .Include(payment => payment.Order)
                .ThenInclude(order => order!.Client)
                .OrderByDescending(payment => payment.Date)
                .ToListAsync());

        return endpoints;
    }
}
