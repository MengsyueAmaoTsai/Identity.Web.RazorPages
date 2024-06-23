using System.Linq.Expressions;

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NSubstitute;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.Identity.Web.Pages.Identity.SignIn;
using RichillCapital.SharedKernel;
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
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            ReturnUrl = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            RememberMe = false,
            ExternalSchemes = [],
            AllowRememberMe = false,
        };

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
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            ReturnUrl = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            RememberMe = false,
            ExternalSchemes = [],
            AllowRememberMe = false,
        };

        var result = await viewModel.OnGetAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_GivenInvalidInput_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            Email = string.Empty,
            Password = string.Empty,
            ReturnUrl = string.Empty,
        };

        var result = await viewModel.OnPostSignInAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_SignInResultIsFailure_Should_ReturnPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            Email = ValidEmail.Value,
            Password = "password",
            ReturnUrl = "/test",
        };

        signInManager.PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>())
            .Returns(Result.Failure(Error.Invalid("Invalid credentials")));

        var result = await viewModel.OnPostSignInAsync();

        await signInManager.Received(1).PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>());

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_AuthorizationRequestIsNullAndReturnUrlIsLocal_Should_RedirectToIndex()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();
        IUrlHelper urlHelper = Substitute.For<IUrlHelper>();

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            Email = ValidEmail.Value,
            Password = "password",
            ReturnUrl = "/test",
            Url = urlHelper,
        };

        signInManager.PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>())
            .Returns(Result.Success);

        var user = User
            .Create(
                UserId.NewUserId(),
                UserName.From("username").Value,
                ValidEmail,
                false,
                "password",
                true,
                0,
                DateTimeOffset.UtcNow)
            .ThrowIfError()
            .Value;

        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.With(user));

        urlHelper.IsLocalUrl(Arg.Any<string>()).Returns(true);

        var result = await viewModel.OnPostSignInAsync();

        await signInManager.Received(1).PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>());

        await userRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<Expression<Func<User, bool>>>(),
            Arg.Any<CancellationToken>());

        await interactionService.Received(1).GetAuthorizationContextAsync(Arg.Any<string>());

        result.Should().BeOfType<RedirectResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_AuthorizationRequestIsNotNullAndIsNativeClient_Should_RedirectToPage()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();
        IUrlHelper urlHelper = Substitute.For<IUrlHelper>();
        var localUrl = "/test";

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            Email = ValidEmail.Value,
            Password = "password",
            ReturnUrl = localUrl,
            Url = urlHelper,
        };

        signInManager.PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>())
            .Returns(Result.Success);

        var user = User
            .Create(
                UserId.NewUserId(),
                UserName.From("username").Value,
                ValidEmail,
                false,
                "password",
                true,
                0,
                DateTimeOffset.UtcNow)
            .ThrowIfError()
            .Value;

        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.With(user));

        interactionService.GetAuthorizationContextAsync(Arg.Any<string>())
            .Returns(new AuthorizationRequest()
            {
                RedirectUri = localUrl,
            });

        urlHelper.IsLocalUrl(Arg.Any<string>()).Returns(true);

        var result = await viewModel.OnPostSignInAsync();

        await signInManager.Received(1).PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>());

        await userRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<Expression<Func<User, bool>>>(),
            Arg.Any<CancellationToken>());

        await interactionService.Received(1).GetAuthorizationContextAsync(Arg.Any<string>());

        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Fact]
    public async Task OnPostSignInAsync_When_AuthorizationRequestIsNotNullAndIsExternalClient_Should_RedirectToUrl()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();
        IUrlHelper urlHelper = Substitute.For<IUrlHelper>();
        var externalUrl = "https://localhost:10000";

        SignInViewModel viewModel = new(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            Email = ValidEmail.Value,
            Password = "password",
            ReturnUrl = externalUrl,
            Url = urlHelper,
        };

        signInManager
            .PasswordSignInAsync(
                Arg.Any<Email>(),
                Arg.Any<string>(),
                Arg.Any<bool>(),
                Arg.Any<bool>())
            .Returns(Result.Success);

        var user = User
            .Create(
                UserId.NewUserId(),
                UserName.From("username").Value,
                ValidEmail,
                false,
                "password",
                true,
                0,
                DateTimeOffset.UtcNow)
            .ThrowIfError()
            .Value;

        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.With(user));

        interactionService.GetAuthorizationContextAsync(Arg.Any<string>())
            .Returns(new AuthorizationRequest()
            {
                RedirectUri = externalUrl,
            });

        urlHelper.IsLocalUrl(Arg.Any<string>()).Returns(true);

        var result = await viewModel.OnPostSignInAsync();

        await signInManager.Received(1).PasswordSignInAsync(
            Arg.Any<Email>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<bool>());

        await userRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<Expression<Func<User, bool>>>(),
            Arg.Any<CancellationToken>());

        await interactionService.Received(1).GetAuthorizationContextAsync(Arg.Any<string>());

        result.Should().BeOfType<RedirectResult>();
    }

    [Fact]
    public async Task OnPostCancelAsync_When_AuthorizationRequestIsNull_Should_RedirectToIndex()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();

        var viewModel = new SignInViewModel(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            ReturnUrl = string.Empty,
            Email = ValidEmail.Value,
            Password = "password",
        };

        var result = await viewModel.OnPostCancelAsync();

        await interactionService.Received(1).GetAuthorizationContextAsync(Arg.Any<string>());

        result.Should().BeOfType<RedirectResult>();
    }

    [Fact]
    public async Task OnPostCancelAsync_When_AuthorizationRequestIsNotNull_Should_RedirectToUrl()
    {
        IAuthenticationSchemeProvider schemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        ISignInManager signInManager = Substitute.For<ISignInManager>();
        IReadOnlyRepository<User> userRepository = Substitute.For<IReadOnlyRepository<User>>();
        IIdentityServerInteractionService interactionService = Substitute.For<IIdentityServerInteractionService>();
        IEventService eventService = Substitute.For<IEventService>();
        var returnUrl = "https://test.com";

        var viewModel = new SignInViewModel(
            schemeProvider,
            signInManager,
            userRepository,
            interactionService,
            eventService)
        {
            ReturnUrl = returnUrl,
            Email = ValidEmail.Value,
            Password = "password",
        };

        interactionService.GetAuthorizationContextAsync(Arg.Any<string>())
            .Returns(new AuthorizationRequest()
            {
                RedirectUri = returnUrl,
            });

        var result = await viewModel.OnPostCancelAsync();
        await interactionService.Received(1).GetAuthorizationContextAsync(Arg.Any<string>());

        result.Should().BeOfType<RedirectResult>();
    }
}