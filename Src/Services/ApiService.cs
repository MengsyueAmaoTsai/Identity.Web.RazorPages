using RichillCapital.Identity.Web.Services.Contracts;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Services;

internal sealed class ApiService(HttpClient _httpClient) :
    IApiService
{
    public async Task<Result<UserResponse>> GetUsersByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/v1/users/{userId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Error
                .Invalid($"Failed to get user with id {userId}")
                .ToResult<UserResponse>();
        }

        var result = await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken);

        return result!.ToResult();
    }

    public async Task<Result<PagedResponse<UserResponse>>> ListUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/v1/users", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Error
                .Invalid("Failed to list users")
                .ToResult<PagedResponse<UserResponse>>();
        }

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<UserResponse>>(cancellationToken);

        return result!.ToResult();
    }
}