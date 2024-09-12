using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Identity;

internal sealed class CurrentUser(
    IHttpContextAccessor _httpContextAccessor) :
    ICurrentUser
{
    private HttpContext? _context;

    public HttpContext Context
    {
        get
        {
            var context = _context ?? _httpContextAccessor?.HttpContext;
            return context is null ?
                throw new InvalidOperationException("HttpContext must not be null.") :
                context;
        }

        set => _context = value;
    }

    public bool IsAuthenticated =>
        Context.User.Identity?.IsAuthenticated ?? false;

    public UserId Id =>
        UserId
            .From(Context.User.FindFirstValue(JwtClaimTypes.Subject) ?? string.Empty)
            .ThrowIfFailure()
            .Value;

    public string Name =>
        Context.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

    public Email Email =>
        Email
            .From(Context.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty)
            .ThrowIfFailure()
            .Value;
}