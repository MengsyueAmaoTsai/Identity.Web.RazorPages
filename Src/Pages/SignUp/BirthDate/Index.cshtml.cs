using System.Globalization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignUp.BirthDate;

public sealed class BirthDateViewModel(
    ILogger<BirthDateViewModel> _logger,
    IUserManager _userManager,
    ISignInManager _signInManager) :
    IdentityViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Region { get; set; } = "Taiwan";

    [BindProperty]
    public required BirthDateModel BirthDate { get; set; } = BirthDateModel.CreateDefault();

    public required IEnumerable<SelectListItem> Regions = 
    [
        new SelectListItem { Value = "Taiwan", Text = "Taiwan" },
        new SelectListItem { Value = "United States", Text = "United States" },
    ];

    public required IEnumerable<SelectListItem> Months = Enumerable.Range(1, 12)
        .Select(month => new SelectListItem
        {
            Value = month.ToString(),
            Text = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month)
        });

    public required IEnumerable<SelectListItem> Days = Enumerable.Range(1, 31)
        .Select(day => new SelectListItem 
        { 
            Value = day.ToString(), 
            Text = day.ToString() 
        });
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState.Values)
            {
                if (state.Errors.Count > 0)
                {
                    _logger.LogError("ModelState is not valid: {State}", state.RawValue);

                    foreach (var error in state.Errors)
                    {
                        _logger.LogError("ModelState error: {Error}", error.ErrorMessage);
                    }

                }
            }

            return Page();
        }

        if (BirthDate.Day == 0)
        {
            ModelState.AddModelError("Day", "Day must be a valid day.");
            return Page();
        }

        if (BirthDate.Month == 0)
        {
            ModelState.AddModelError("Month", "Month must be a valid month.");
            return Page();
        }

        // Create the user
        TempData.TryGetValue("Password", out var password);

        var user = Domain.Users.User
            .Create(
                UserId.NewUserId(),
                UserName.From(EmailAddress).ThrowIfFailure().ValueOrDefault,
                Email.From(EmailAddress).ThrowIfFailure().ValueOrDefault,
                emailConfirmed: true,
                passwordHash: password as string ?? string.Empty,
                lockoutEnabled: true,
                accessFailedCount: 0,
                DateTimeOffset.UtcNow)
            .ThrowIfError()
            .ValueOrDefault;

        var createResult = await _userManager.CreateAsync(user, cancellationToken);

        if (createResult.IsFailure)
        {
            return RedirectToErrorPage();
        }

        var signInResult = await _signInManager.SignInAsync(
            user, 
            isPersistent: false, 
            cancellationToken: cancellationToken);

        if (signInResult.IsFailure)
        {
            return RedirectToErrorPage();
        }

        return RedirectToProfilePage();
    }
}


public sealed record BirthDateModel
{
    public required int Year { get; init; }
    public required int Month { get; init; }
    public required int Day { get; init; }

    public static BirthDateModel CreateDefault() => new BirthDateModel
    {
        Year = DateTimeOffset.UtcNow.Year,
        Month = 1,
        Day = 1,
    };
}