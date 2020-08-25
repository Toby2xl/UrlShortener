using System.Collections;
using System.Linq;
using System.Data;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RepoDb;
using Urlinker.Entities;
using Urlinker.Interfaces;
using System.Threading.Tasks;
using Urlinker.Config;
using Microsoft.Extensions.Options;

namespace Urlinker.Repositories
{
    public class UrlRepo : IUrlRepo
    {
        private readonly IConfiguration _configuration;
        
        //Used to access the Secret store ConnectionString for PostgreSql
        private UrlSecretSettings _urlSettings;

        public UrlRepo(IConfiguration configuration, IOptions<UrlSecretSettings> urlSettings)
        {
            _configuration = configuration;
            _urlSettings = urlSettings.Value;
        }

        public UrlRepo()
        {
        }

        public async Task CreateUrlAsync(Ulinker newUrl)
        {
            //using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            await connection.InsertAsync<Ulinker>(newUrl);   
        }

        public async Task<int> DeleteAsync(Ulinker existingUrl)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var affectedRows = await connection.DeleteAsync<Ulinker>(existingUrl);
            return affectedRows;
        }

        public async Task<IReadOnlyCollection<Ulinker>> GetAllUrlAsync()
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var urList = await  connection.QueryAllAsync<Ulinker>();
            return (IReadOnlyCollection<Ulinker>)urList.AsQueryable().OrderBy(url => url.createdDate);
        }

        public async Task<IEnumerable<Ulinker>> GetAllUrlA()
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var urList = await connection.QueryAllAsync<Ulinker>();
            // return urList.AsQueryable().OrderBy(u => u.createdDate);
            return urList.OrderBy(m=> m.createdDate).ToList();

        }

        public async Task<bool> isAnyAsync(string uName)
        {
            //TODO: Add a Caching Mechanism to this call.......
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var urlName = await connection.QueryAsync<Ulinker>(u => u.UrlShortName == uName);

            return urlName.Any(o => o.UrlShortName == uName) ? true : false;
        }

        public async Task<bool> ExistsAsync(string shortName)
        {
            //TODO: Cache the returned value for faster subsequent access.........

            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var isExisting = await connection.ExistsAsync<Ulinker>(u=> u.UrlShortName == shortName);
            return isExisting;
        }

        public Ulinker GetById(Guid id)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = connection.Query<Ulinker>(url => url.id == id).FirstOrDefault();
            return Url;
        }

        public string GetShortenUrlByName(string urlShortName)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = connection.Query<Ulinker>(url => url.UrlShortName == urlShortName).FirstOrDefault();
            return Url.ShortenUrl;

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlName"></param>
        /// <returns></returns>
        public async Task<string> GetShortenUrlByNameAsync(string urlName)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = await connection.QueryAsync<Ulinker>(url => url.UrlShortName == urlName);
            return Url.FirstOrDefault().ShortenUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Ulinker> GetUrlbyIdAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = await connection.QueryAsync<Ulinker>(url => url.id == id);
            return Url.SingleOrDefault();
        }

        public async Task<int> UpdateAsync(Ulinker existingUrl)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var affectedRows = await connection.UpdateAsync<Ulinker>(existingUrl);
            return affectedRows;
            
        }

        public string GetOriginalUrlByName(string urlShortName)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = connection.Query<Ulinker>(url => url.UrlShortName == urlShortName).FirstOrDefault();
            return Url.OriginalUrl;
        }

        public async Task<string> GetOriginalUrlByNameAsync(string urlName)
        {
            using var connection = new NpgsqlConnection(_urlSettings.ConnString);
            var Url = await connection.QueryAsync<Ulinker>(url => url.UrlShortName == urlName);
            return Url.FirstOrDefault().OriginalUrl;
        }
    }
}