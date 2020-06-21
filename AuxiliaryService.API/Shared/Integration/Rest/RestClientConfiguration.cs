using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Integration.Rest
{
    public enum AuthType
    {
        No, 
        Basic,
        BearerToken
    }

    public static class ContentType
    {
        public const string ApplicationJson = "application/json";
    }

    public class RestClientConfiguration
    {
        /// <summary>
        /// rest WS uri
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// media type
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// auth type
        /// </summary>
        public AuthType AuthType { get; set; }

        /// <summary>
        /// username if Auth = Basic
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password if Auth = Basic
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Token if Auth = Bearer Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// if auto decompression is applied
        /// </summary>
        public bool DeflateGzipDecompression { get; set; }

        public RestClientConfiguration()
        {
            AuthType = AuthType.No;
            DeflateGzipDecompression = false;
        }
    }
}
