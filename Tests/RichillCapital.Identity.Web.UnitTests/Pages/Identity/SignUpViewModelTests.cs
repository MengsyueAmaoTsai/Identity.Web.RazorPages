using System.Linq.Expressions;

using FluentAssertions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.Identity.Web.Pages.Identity;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Identity;

public sealed class SignUpViewModelTests
{
    private static readonly Email ValidEmail = Email.From("test@gmail.com").ThrowIfFailure().Value;

    [Fact]
    public async Task OnGetAsync_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IUserManager userManager = Substitute.For<IUserManager>();

        var viewModel = new SignUpViewModel(schemeProvider, userRepository, userManager)
        {
            Name = "test",
            Email = ValidEmail.Value,
            Password = "test",
            ConfirmPassword = "test",
            ReturnUrl = "/test",
        };

        var result = await viewModel.OnGetAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostAsync_When_GivenDuplicateEmail_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IUserManager userManager = Substitute.For<IUserManager>();

        userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var viewModel = new SignUpViewModel(schemeProvider, userRepository, userManager)
        {
            Name = "test",
            Email = ValidEmail.Value,
            Password = "test",
            ConfirmPassword = "test",
            ReturnUrl = "/test"
        };

        var result = await viewModel.OnPostAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostAsync_WhenCreateFails_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IUserManager userManager = Substitute.For<IUserManager>();

        userRepository.AnyAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure(Error.Invalid("")));

        var viewModel = new SignUpViewModel(schemeProvider, userRepository, userManager)
        {
            Name = "test",
            Email = ValidEmail.Value,
            Password = "test",
            ConfirmPassword = "test",
            ReturnUrl = "/test",
        };

        var result = await viewModel.OnPostAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostAsync_When_CreateSuccess_Should_ReturnLocalRedirectResult()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IUserManager userManager = Substitute.For<IUserManager>();
        IUrlHelper urlHelper = Substitute.For<IUrlHelper>();

        userRepository
            .AnyAsync(
                Arg.Any<Expression<Func<User, bool>>>(), 
                Arg.Any<CancellationToken>())
            .Returns(false);

        userManager
            .CreateAsync(
                Arg.Any<User>(), 
                Arg.Any<string>(), 
                Arg.Any<CancellationToken>())
            .Returns(Result.Success);

        var viewModel = new SignUpViewModel(schemeProvider, userRepository, userManager)
        {
            Name = "test",
            Email = ValidEmail.Value,
            Password = "test",
            ConfirmPassword = "test",
            ReturnUrl = "/test",
            Url = urlHelper,
        };

        var result = await viewModel.OnPostAsync();

        result.Should().BeOfType<LocalRedirectResult>();
    }
}
