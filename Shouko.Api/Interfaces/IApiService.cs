using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.Api.Interfaces;

public interface IApiService
{
    Task<ApiResult<ApiResponseDto>> SendRequest(CreateThreadDto requestDto);
}