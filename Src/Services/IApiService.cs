using RichillCapital.Identity.Web.Services.Contracts;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Services;

public interface IApiService
{
    Task<Result<PagedResponse<UserResponse>>> ListUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetUsersByIdAsync(string userId, CancellationToken cancellationToken = default);
}
