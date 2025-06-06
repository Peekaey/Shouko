using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IApiServiceBusinessService
{
    Task<InteractionResult> CreateApiTextThread<T>(CreateThreadDto createThreadDto) where T : class;
    Task<InteractionResult> CreateApiImageThread<T>(CreateThreadDto createThreadDto) where T : class;
}