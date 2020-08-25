using System.Threading.Tasks;
using Urlinker.Dto;
using Urlinker.Dto.Responses;

namespace Urlinker.Interfaces
{
    public interface IUrlService
    {
        Task<string> GenerateShortName();

        Task<UrlResponse> AddUrl(UrlRequestDto urlRequest);

        Task<string> GetOriginalUrlByNameAsync(string urlShortName);
        string GetOriginalUrlByName(string urlShortName);

        Task<string> GetShortenUrlByNameAsync(string urlShortName);

        string GetShortenUrlByName(string urlShortName);

    }
}