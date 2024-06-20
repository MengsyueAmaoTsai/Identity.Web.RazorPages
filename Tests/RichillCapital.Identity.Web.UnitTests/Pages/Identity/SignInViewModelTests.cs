using FluentAssertions;

using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Pages.Identity;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignInViewModelTests
{
    public SignInViewModelTests()
    {
    }

    [Fact]
    public async Task OnGetAsync_Should_ReturnPage()
    {
        var viewModel = new SignInViewModel()
        {
            ReturnUrl = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            RememberMe = false,
            ExternalSchemes = [],
            AllowRememberMe = false,
        };

        var result = await viewModel.OnGetAsync(CancellationToken.None);

        result.Should().BeOfType<PageResult>();
    }
}