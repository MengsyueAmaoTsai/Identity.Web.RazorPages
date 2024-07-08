using Duende.IdentityServer.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.Identity.Web.Pages.Identity.SignIn;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignInViewModelTests
{
    private static readonly Email ValidEmail = Email.From("test@gmail.com").ThrowIfFailure().Value;

    public SignInViewModelTests()
    {
    }

    [Fact]
    public void Constructor_Should_InitializeProperties()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            userRepository,
            interactionService)
        {
            ReturnUrl = string.Empty,
            EmailAddress = string.Empty,
            ExternalSchemes = [],
        };

        viewModel.ReturnUrl.Should().BeNullOrEmpty();
        viewModel.EmailAddress.Should().BeNullOrEmpty();
        viewModel.ExternalSchemes.Should().BeEmpty();
    }

    [Fact]
    public async Task OnGetAsync_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            userRepository,
            interactionService)
        {
            ReturnUrl = string.Empty,
            EmailAddress = string.Empty,
            ExternalSchemes = [],
        };

        var result = await viewModel.OnGetAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_GivenInvalidInput_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            userRepository,
            interactionService)
        {
            EmailAddress = string.Empty,
            ReturnUrl = string.Empty,
        };

        var result = await viewModel.OnPostAsync();

        result.Should().BeOfType<PageResult>();
    }
}