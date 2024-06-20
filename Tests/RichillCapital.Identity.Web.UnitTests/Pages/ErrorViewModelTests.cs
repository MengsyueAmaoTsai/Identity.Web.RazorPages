using FluentAssertions;

using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Pages.Error;

namespace RichillCapital.Identity.Web.UnitTests.Pages;

public sealed class ErrorViewModelTests
{
    [Fact]
    public void OnGet_Should_ReturnPage()
    {
        var viewModel = new ErrorViewModel()
        {
            ErrorId = "errorId",
            TraceId = "traceId",
            ErrorMessage = "errorMessage",
        };

        var result = viewModel.OnGet();

        result.Should().BeOfType<PageResult>();
    }
}