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
            ErrorMessage = "errorMessage",
            ErrorCode = "errorCode",
            RequestId = "requestId",
            CorrelationId = "correlationId",
            Timestamp = DateTimeOffset.UtcNow,
        };

        var result = viewModel.OnGet();

        result.Should().BeOfType<PageResult>();
    }
}