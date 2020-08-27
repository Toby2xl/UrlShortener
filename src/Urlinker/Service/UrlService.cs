using System;
using System.Threading.Tasks;
using Urlinker.Dto;
using Urlinker.Entities;
using Urlinker.Extension;
using Urlinker.Interfaces;
using Urlinker.Dto.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Urlinker.Service
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepo _urlRepo;

        private readonly ILogger<UrlService> _logger;
        private readonly IHttpContextAccessor _accessor;

        public UrlService(IUrlRepo urlRepo, ILogger<UrlService> logger, IHttpContextAccessor  accessor)
        {
            _urlRepo = urlRepo;
            _logger = logger;
            _accessor = accessor;
        }
        public async Task<UrlAddResponse> AddUrlAsync(string newUrlToAdd)
        {
            var validUrl = newUrlToAdd;
            var response = new UrlAddResponse();
            if (!validUrl.isValidUrl())
            {
                response.IsSuccess = false;
                response.Message = $"The target url - { validUrl} is not valid";
                return response;
            }
            try
            {
                var newUrl = await mapToDomainAsync(newUrlToAdd);
                await _urlRepo.CreateUrlAsync(newUrl);
                response.IsSuccess = true;
                response.Message = "Successfully created the url";
                response.newShortenUrl = newUrl.ShortenUrl;
                _logger.LogInformation(message: $"New Shorten Url with id{newUrl.id} created successfully");
            }
            catch (Exception ex)
            {          
                response.IsSuccess = false;
                response.Message = "An error occured while generating the Url";
                _logger.LogCritical(message: $"Error occured creating Url.......{ex}");
            }
            return response;
        }


        public async Task<string> GenerateShortName()
        {
            var randNumber = new Random().Next(6, 13);
            var UrlName = createRandomString(randNumber);
            var generatedShortName = ""; 
            while(true)
            {
                if(await _urlRepo.ExistsAsync(UrlName) == true)
                {
                    UrlName = createRandomString(randNumber);
                    continue;
                }
                else
                {
                    generatedShortName = UrlName;
                    break;
                }
            }
            return generatedShortName;
        }

        public string GetOriginalUrlByName(string urlShortName)
        {
            var originalUrl = _urlRepo.GetOriginalUrlByName(urlShortName);
            return originalUrl != null ? originalUrl : "";
        }

        public async Task<UrlGetResponse> GetOriginalUrlByNameAsync(string urlShortName)
        {
            var response = new UrlGetResponse();
            var originalUrl = await _urlRepo.GetOriginalUrlByNameAsync(urlShortName);
            if(originalUrl is null)
            {
                response.IsSuccess = false;
                response.Message = $"There's no such link in existence....";
            }
            response.OriginalUrl = originalUrl;
            return response;
        }

        public string GetShortenUrlByName(string urlShortName)
        {
            var shortenUrl = _urlRepo.GetShortenUrlByName(urlShortName);
            return shortenUrl != null ? shortenUrl : "";
        }

        public async Task<string> GetShortenUrlByNameAsync(string urlShortName)
        {   
           var shortenUrl = await _urlRepo.GetShortenUrlByNameAsync(urlShortName);
           return shortenUrl != null ? shortenUrl : " ";
           
        }

        /// <summary>
        /// Generates a random string of specific a lenght. The default length is 8 characters.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string createRandomString(int num = 8)
        {
            const string alphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var shortName = alphaNum.Random(num);

            return shortName;
        }

        /// <summary>
        /// Maps the Url Request object to the Domain Entities
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <returns>Ulinker object</returns>
        private async Task<Ulinker> mapToDomainAsync(string originalUrl)
        {
            string shortUrlName = await GenerateShortName();
            var baseUrl = $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host.Value.ToString()}";

            var newUrl = new Ulinker
            {
                id = Guid.NewGuid(),
                UrlShortName = shortUrlName,
                OriginalUrl = originalUrl,
                ShortenUrl = $"{baseUrl}/{shortUrlName}",
                createdDate = DateTimeOffset.UtcNow.DateTime
            };
            return newUrl;
        }

        /*
        $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value.ToString()}";
        private Ulinker mapToDomain(UrlRequestDto url)
        {
            var newUrl = new  Ulinker
            {
                id = Guid.NewGuid(),
                UrlShortName =  GenerateShortName().GetAwaiter().GetResult(),
                OriginalUrl = url.OriginalUrl,
                ShortenUrl = "",
                createdDate = DateTime.UtcNow
            };
            return newUrl;
        }*/


    }
}