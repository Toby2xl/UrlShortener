using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Urlinker.Dto;
using Urlinker.Entities;
using Urlinker.Extension;
using Urlinker.Interfaces;
using Urlinker.Dto.Responses;

namespace Urlinker.Service
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepo _urlRepo;

        public UrlService(IUrlRepo urlRepo)
        {
            _urlRepo = urlRepo;
        }
        public async Task<UrlResponse> AddUrl(UrlRequestDto urlRequest)
        {
            var validUrl = urlRequest.OriginalUrl;

            var response = new UrlResponse();

            if (!validUrl.isValidUrl())
            {
                //throw new ArgumentException($"The target url- {validUrl} is not valid");
                response.IsSuccess = false;
                response.message = $"The target url - { validUrl} is not valid";

                return response;
            }
            else
            {
                var newUrl = await mapToDomainAsync(urlRequest);
                await _urlRepo.CreateUrlAsync(newUrl);
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

        public async Task<string> GetOriginalUrlByNameAsync(string urlShortName)
        {
            var originalUrl = await _urlRepo.GetOriginalUrlByNameAsync(urlShortName);
            return originalUrl != null ? originalUrl : "";
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
        private async Task<Ulinker> mapToDomainAsync(UrlRequestDto urlRequest)
        {
            var newUrl = new Ulinker
            {
                id = Guid.NewGuid(),
                UrlShortName = await GenerateShortName(),
                OriginalUrl = urlRequest.OriginalUrl,
                ShortenUrl = " ",
                createdDate = DateTimeOffset.UtcNow.DateTime
            };
            return newUrl;
        }

        /*
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