using System.Security.Claims;

using FluentAssertions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Identity.Web.Pages;

namespace RichillCapital.Identity.Web.UnitTests.Pages;

public sealed class IndexViewModelTests
{
    private readonly IAuthenticationService _authenticationService = Substitute.For<IAuthenticationService>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

    public IndexViewModelTests()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IAuthenticationService))
            .Returns(_authenticationService);

        _httpContext.RequestServices.Returns(serviceProvider);
    }

    [Fact]
    public async Task OnGetAsync_When_AuthenticationFails_Should_RedirectToErrorPage()
    {
        // Arrange
        _authenticationService
            .AuthenticateAsync(_httpContext, null)
            .Returns(Task.FromResult(AuthenticateResult.Fail("Authentication failed")));

        var pageModel = new IndexViewModel()
        {
            PageContext = new PageContext
            {
                HttpContext = _httpContext,
            },

            Properties = null!,
        };

        // Act
        var result = await pageModel.OnGetAsync(CancellationToken.None);

        // Assert
        result.Should().BeOfType<RedirectToPageResult>()
            .Which.PageName.Should().Be("/identity/signIn");
    }

    [Fact]
    public async Task OnGetAsync_When_AuthenticationSucceeds_Should_ReturnPage()
    {
        // Arrange
        var properties = new AuthenticationProperties();

        _authenticationService
            .AuthenticateAsync(_httpContext, null)
            .Returns(Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(),
                properties,
                "scheme"))));

        var pageModel = new IndexViewModel()
        {
            PageContext = new PageContext
            {
                HttpContext = _httpContext,
            },

            Properties = null!,
        };

        // Act
        var result = await pageModel.OnGetAsync(CancellationToken.None);

        // Assert
        result.Should().BeOfType<PageResult>();
        pageModel.Properties.Should().Be(properties);
    }
}
