using RichillCapital.Identity.Web.Services.Contracts;
using RichillCapital.Identity.Web.Services.Contracts.Users;
using RichillCapital.Identity.Web.Services.Contracts.Files;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Services;

public interface IApiService
{
    Task<Result<PagedResponse<UserResponse>>> ListUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<FileEntryResponse>>> ListFileEntriesAsync(CancellationToken cancellationToken = default);
}
