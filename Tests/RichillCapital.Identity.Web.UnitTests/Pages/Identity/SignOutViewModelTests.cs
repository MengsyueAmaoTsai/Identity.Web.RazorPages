using Duende.IdentityServer.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Identity.Web.Pages.Identity.SignOut;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignOutViewModelTests
{
    [Fact]
    public async Task OnPostSignOutAsync_When_CurrentUserIsNotAuthenticated_Should_RedirectToSingedOutPage()
    {
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        ICurrentUser currentUser = Substitute.For<ICurrentUser>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();

        var viewModel = new SignOutViewModel(interactionService, currentUser, signInManager);

        var result = await viewModel.OnPostAsync("/test");

        currentUser.IsAuthenticated.Returns(false);

        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Fact]
    public async Task OnPostSignOutAsync_When_SignOutFails_Should_ReturnPage()
    {
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        ICurrentUser currentUser = Substitute.For<ICurrentUser>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();

        var viewModel = new SignOutViewModel(interactionService, currentUser, signInManager);
        currentUser.IsAuthenticated.Returns(true);
        signInManager.SignOutAsync(default).Returns(Result.Failure(Error.Unexpected("Unexpected error")));

        var result = await viewModel.OnPostAsync("/test");

        result.Should().BeOfType<PageResult>();

        await signInManager.Received(1).SignOutAsync(default);
    }

    [Fact]
    public async Task OnPostSignOutAsync_When_SignOutSuccessAndProviderIsExternal_Should_RedirectToSignedOutPage()
    {
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        ICurrentUser currentUser = Substitute.For<ICurrentUser>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();

        var viewModel = new SignOutViewModel(interactionService, currentUser, signInManager);
        currentUser.IsAuthenticated.Returns(true);
        signInManager.SignOutAsync(default).Returns(Result.Success);

        // TODO: Act
    }
}