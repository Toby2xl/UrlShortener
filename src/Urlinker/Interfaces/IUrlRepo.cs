using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Urlinker.Entities;

namespace Urlinker.Interfaces
{
    public interface IUrlRepo
    {
        Task<IReadOnlyCollection<Ulinker>> GetAllUrlAsync();
        Ulinker GetById(Guid id);
        //string GetByName(string urlShortName);
        string GetShortenUrlByName(string urlShortName);
        Task<string> GetShortenUrlByNameAsync(string urlName);
        string GetOriginalUrlByName(string urlShortName);
        Task<string> GetOriginalUrlByNameAsync(string urlName);
        Task<Ulinker> GetUrlbyIdAsync(Guid id);
        Task<bool> ExistsAsync(string shortName);
        Task<bool> isAnyAsync(string uName);
        Task CreateUrlAsync(Ulinker newUrl);
        Task<int> UpdateAsync(Ulinker existingUrl);
        Task<int> DeleteAsync(Ulinker existingUrl);
    }
}