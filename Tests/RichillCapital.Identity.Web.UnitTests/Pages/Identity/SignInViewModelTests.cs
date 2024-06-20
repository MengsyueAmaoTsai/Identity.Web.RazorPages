using FluentAssertions;

using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Identity.Web.Pages.Identity;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignInViewModelTests
{
    public SignInViewModelTests()
    {
    }

    [Fact]
    public void Constructor_Should_InitializeProperties()
    {
        var signInManager = Substitute.For<ISignInManager>();
        var viewModel = new SignInViewModel(signInManager);

        viewModel.ReturnUrl.Should().BeNullOrEmpty();
        viewModel.Email.Should().BeNullOrEmpty();
        viewModel.Password.Should().BeNullOrEmpty();
        viewModel.RememberMe.Should().BeFalse();
        viewModel.ExternalSchemes.Should().BeEmpty();
        viewModel.AllowRememberMe.Should().BeFalse();
    }

    [Fact]
    public async Task OnGetAsync_Should_ReturnPage()
    {
        var signInManager = Substitute.For<ISignInManager>();
        var viewModel = new SignInViewModel(signInManager)
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

    [Fact]
    public async Task OnPostAsync_When_InvalidInput_Should_ReturnPage()
    {
        var signInManager = Substitute.For<ISignInManager>();
        var viewModel = new SignInViewModel(signInManager)
        {
            Email = "invalid-email",
            Password = "invalid-password",
        };

        var action = "SignIn";

        var result = await viewModel.OnPostAsync(action, CancellationToken.None);

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostAsync_When_SignInFailed_Should_ReturnPage()
    {
        var signInManager = Substitute.For<ISignInManager>();
        var viewModel = new SignInViewModel(signInManager);

        var action = "SignIn";

        var result = await viewModel.OnPostAsync(action, CancellationToken.None);

        result.Should().BeOfType<PageResult>();
    }
}