using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.SharedKernel.Specifications;
using RichillCapital.SharedKernel.Specifications.Builders;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile.Accounts;

[Authorize]
public sealed class AccountsViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) : 
    PageModel
{
    public required IEnumerable<AccountModel> Accounts { get; set; } = [];
    
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var maybeUser = await _userRepository.FirstOrDefaultAsync(
            new UserWithAccountsSpecification(_currentUser.Id), 
            cancellationToken).ThrowIfNull();

        var user = maybeUser.Value;
        
        var accounts = user.Accounts.Select(account => new AccountModel
        {
            Id = account.Id.Value,
            Name = account.Name,
        });

        Accounts = accounts;

        return Page();
    }
}

internal sealed class UserWithAccountsSpecification : Specification<User>
{
    public UserWithAccountsSpecification(UserId userId)
    {
        Query.Where(u => u.Id == userId).Include(u => u.Accounts);
    }
}

public sealed record AccountModel
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}