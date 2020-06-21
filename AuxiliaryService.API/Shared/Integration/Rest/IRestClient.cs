using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Integration.Rest
{
    public interface IRestClient
    {
        /// <summary>
        /// post message to REST entry point
        /// </summary>
        /// <param name="message"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<string> PostAsync(string message, RestClientConfiguration configuration);

        /// <summary>
        /// post stream as multipart data to REST
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<string> PostStreamAsync(Stream stream, string filename, RestClientConfiguration configuration);

        /// <summary>
        /// invoke delete HTTP METHOD
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task DeleteAsync(RestClientConfiguration configuration);

        /// <summary>
        /// get stream
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<Stream> GetStreamAsync(RestClientConfiguration configuration);

        /// <summary>
        /// get string
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(RestClientConfiguration configuration);

    }
}
