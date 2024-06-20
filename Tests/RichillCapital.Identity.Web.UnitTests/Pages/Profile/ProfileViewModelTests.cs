using NSubstitute;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.UnitTests.Pages.Profile;

public sealed class ProfileViewModelTests
{
    public async Task OnGetAsync_Should_ReturnPage()
    {
        IUserManager userManager = Substitute.For<IUserManager>();
        ICurrentUser currentUser = Substitute.For<ICurrentUser>();
    }
}
