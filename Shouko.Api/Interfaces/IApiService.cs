using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.Api.Interfaces;

public interface IApiService
{
    Task<ApiResult<T>> SendRequest<T>(CreateThreadDto requestDto) where T : class;
}