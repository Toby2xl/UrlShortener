using System;
using System.Text;

namespace Urlinker.Extension
{
    public static class ValidUrl
    {
        public static bool isValidUrl(this string source) => Uri.TryCreate(source, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        public static string Random(this string chars, int length = 8)
        {
            var randomString = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[random.Next(chars.Length)]);
            }
                

            return randomString.ToString();
        }

        public static bool isNotDomainAddress(this string url)
        {
            //gets the index of the last "." in the url
            int urlIndedx = url.LastIndexOf(".");
            var spanUrl = url.AsSpan();

            var slicedUrl = spanUrl.Slice(urlIndedx + 1);

           /* if (slicedUrl.Length > 5)
            {
                if (slicedUrl.IndexOfAny("/") > 1)
                {
                    return slicedUrl.Slice(slicedUrl.IndexOf("/") + 1).Length > 0 ? true : false;
                }
                return true;
            } */

            if(slicedUrl.Length > 5 && slicedUrl.IndexOfAny("/") > 1)
            {
                return slicedUrl.Slice(slicedUrl.IndexOf("/") + 1).Length > 0 ? true : false;
            }
            return false;
        }










    }

    
}