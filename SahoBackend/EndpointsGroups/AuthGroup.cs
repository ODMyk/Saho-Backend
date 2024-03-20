using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class AuthGroup
{
    public static void AddEndpoints(this WebApplication app) {
        var group = app.MapGroup("/auth");

        group.MapPost("/", AuthEndpoints.Login);
        group.MapPost("/new", AuthEndpoints.Register);
        group.MapGet("/", AuthEndpoints.GetMe).RequireAuthorization(new AuthorizeAttribute {Policy = "UserPolicy"});
    }    
}