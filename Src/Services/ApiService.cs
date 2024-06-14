using System.Net.Http.Headers;

using RichillCapital.Identity.Web.Services.Contracts;
using RichillCapital.Identity.Web.Services.Contracts.Files;
using RichillCapital.Identity.Web.Services.Contracts.Users;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Services;

internal sealed class ApiService(
    HttpClient _httpClient) :
    IApiService
{
    public async Task<Result<UserResponse>> GetUserByIdAsync(
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

    public async Task<Result<IEnumerable<FileEntryResponse>>> ListFileEntriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/v1/files", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Error
                .Invalid("Failed to list file entries")
                .ToResult<IEnumerable<FileEntryResponse>>();
        }

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<FileEntryResponse>>(cancellationToken);

        return result!.ToResult();
    }

    public async Task<Result<Guid>> UploadFileAsync(
        UploadFileRequest requset,
        CancellationToken cancellationToken = default)
    {
        using var fileStream = requset.File.OpenReadStream();

        var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue(requset.File.ContentType);

        using var content = new MultipartFormDataContent
        {
            { new StringContent(requset.Name), "Name" },
            { new StringContent(requset.Description), "Description" },
            { new StringContent(requset.Encrypt.ToString()), "Encrypt" },
            { fileContent, "File", requset.File.FileName }
        };

        var response = await _httpClient.PostAsync(
            "api/v1/files", 
            content, 
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Error
                .Invalid("Failed to upload file")
                .ToResult<Guid>();
        }

        var result = await response.Content.ReadFromJsonAsync<UploadFileResponse>(cancellationToken);

        return result!.Id.ToResult();
    }
}