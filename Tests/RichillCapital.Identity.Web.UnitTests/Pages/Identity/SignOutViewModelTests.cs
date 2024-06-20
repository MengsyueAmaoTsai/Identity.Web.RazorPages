using Duende.IdentityServer.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using RichillCapital.Identity.Web.Pages.Identity;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignOutViewModelTests
{
    [Fact]
    public async Task OnPostSignOutAsync_When_CurrentUserIsNotAuthenticated_Should_RedirectToSingedOutPage()
    {
        ICurrentUser currentUser = Substitute.For<ICurrentUser>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();

        currentUser.IsAuthenticated.Returns(false);

        var viewModel = new SignOutViewModel(currentUser, interactionService);

        var result = await viewModel.OnPostAsync("/test");

        result.Should().BeOfType<RedirectToPageResult>();
    }
}