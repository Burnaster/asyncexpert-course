using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitExercises.Core
{
    public class AsyncHelpers
    {
        private const int InitialRetryDelay = 1000;

        public static Task<string> GetStringWithRetries(HttpClient client, string url, int maxTries = 3, CancellationToken token = default)
        {
            // Create a method that will try to get a response from a given `url`, retrying `maxTries` number of times.
            // It should wait one second before the second try, and double the wait time before every successive retry
            // (so pauses before retries will be 1, 2, 4, 8, ... seconds).
            // * `maxTries` must be at least 2
            // * we retry if:
            //    * we get non-successful status code (outside of 200-299 range), or
            //    * HTTP call thrown an exception (like network connectivity or DNS issue)
            // * token should be able to cancel both HTTP call and the retry delay
            // * if all retries fails, the method should throw the exception of the last try
            // HINTS:
            // * `HttpClient.GetAsync` does not accept cancellation token (use `GetAsync` instead)
            // * you may use `EnsureSuccessStatusCode()` method
            if (maxTries < 2)
            {
                throw new ArgumentException();
            }

            return GetStringWithRetriesInternal(client, url, maxTries, token);
        }

        private static async Task<string> GetStringWithRetriesInternal(HttpClient client, string url, int maxTries, CancellationToken token)
        {
            HttpResponseMessage response;
            int retryDelay = default;

            for (int i = 0; i <= maxTries; i++)
            {
                try
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }

                    if (retryDelay != default)
                    {
                        await Task.Delay(retryDelay, token);
                    }

                    response = await client.GetAsync(url, token);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException)
                {
                    retryDelay = retryDelay != default ? retryDelay * 2 : InitialRetryDelay;
                }
            }

            throw new HttpRequestException($"Maximum number of retries of {maxTries} reached.");
        }
    }
}
