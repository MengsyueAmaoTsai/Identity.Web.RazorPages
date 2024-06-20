using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.Identity.Web.Pages.Identity;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignInViewModelTests
{
    public SignInViewModelTests()
    {
    }

    [Fact]
    public void Constructor_Should_InitializeProperties()
    {
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var signInManager = Substitute.For<ISignInManager>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService);

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
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var signInManager = Substitute.For<ISignInManager>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService)
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
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var signInManager = Substitute.For<ISignInManager>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService)
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
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var signInManager = Substitute.For<ISignInManager>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService);

        var action = "SignIn";

        var result = await viewModel.OnPostAsync(action, CancellationToken.None);

        await signInManager.Received(1)
            .PasswordSignInAsync(
                Arg.Any<Email>(),
                Arg.Any<string>(),
                Arg.Any<bool>(),
                Arg.Any<bool>());

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostAsync_When_SignInSuccessAndIsLocalUrl_Should_RedirectToIndex()
    {
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var signInManager = Substitute.For<ISignInManager>();

        interactionService.GetAuthorizationContextAsync(Arg.Any<string>())
            .Returns(new AuthorizationRequest()
            {
                RedirectUri = "/return-url",
            });

        signInManager.PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>()).
            Returns(Task.FromResult(Result.Success));

        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService)
        {
            ReturnUrl = "/return-url",
            Email = "valid-email",
            Password = "valid-password",
        };

        var action = "SignIn";

        var result = await viewModel.OnPostAsync(action, CancellationToken.None);

        await signInManager.Received(1)
            .PasswordSignInAsync(
                Arg.Any<Email>(),
                Arg.Any<string>(),
                Arg.Any<bool>(),
                Arg.Any<bool>());

        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Fact]
    public async Task OnPostAsync_When_SignInSuccessAndIsExternalUrl_Should_RedirectToReturnUrl()
    {
        var interactionService = Substitute.For<IIdentityServerInteractionService>();
        var userRepository = Substitute.For<IReadOnlyRepository<User>>();
        var signInManager = Substitute.For<ISignInManager>();

        interactionService.GetAuthorizationContextAsync(Arg.Any<string>())
            .Returns(new AuthorizationRequest()
            {
                RedirectUri = "https://www.google.com",
            });

        signInManager.PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>()).
            Returns(Task.FromResult(Result.Success));

        var viewModel = new SignInViewModel(signInManager, userRepository, interactionService)
        {
            ReturnUrl = "/return-url",
            Email = "valid-email",
            Password = "valid-password",
        };

        var action = "SignIn";

        var result = await viewModel.OnPostAsync(action, CancellationToken.None);

        await signInManager.Received(1)
            .PasswordSignInAsync(
                Arg.Any<Email>(),
                Arg.Any<string>(),
                Arg.Any<bool>(),
                Arg.Any<bool>());

        result.Should().BeOfType<RedirectResult>();
    }
}