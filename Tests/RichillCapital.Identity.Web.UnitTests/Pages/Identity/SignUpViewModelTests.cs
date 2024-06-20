using FluentAssertions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.Identity.Web.Pages.Identity;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignUpViewModelTests
{
    [Fact]
    public async Task OnGetAsync_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IUserManager userManager = Substitute.For<IUserManager>();

        var viewModel = new SignUpViewModel(schemeProvider, userRepository, userManager)
        {
            ReturnUrl = "/test",
        };

        var result = await viewModel.OnGetAsync();

        result.Should().BeOfType<PageResult>();
    }
}
