using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Alza_WebApi.Api.UnitTests.Extension
{
    /// <summary>
    /// Extension class for <see cref="HttpClient"/>
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to a Uri designated as a string with the given <paramref name="value"/> serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return client.PatchAsync(requestUri, new ObjectContent<T>(value, new JsonMediaTypeFormatter()));
        }
    }
}
