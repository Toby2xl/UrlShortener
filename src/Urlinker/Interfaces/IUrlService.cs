using System.Threading.Tasks;
using Urlinker.Dto;
using Urlinker.Dto.Responses;

namespace Urlinker.Interfaces
{
    public interface IUrlService
    {
        Task<string> GenerateShortName();

        Task<UrlAddResponse> AddUrlAsync(string newUrl);

        Task<UrlGetResponse> GetOriginalUrlByNameAsync(string urlShortName);
        string GetOriginalUrlByName(string urlShortName);

        Task<string> GetShortenUrlByNameAsync(string urlShortName);

        string GetShortenUrlByName(string urlShortName);

    }
}