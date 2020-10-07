using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateService.Services.Utils
{
	internal static class HttpRequestLauncher
	{
		internal static async Task<T> GetAsync<T>(string requestUrl) where T : class
		{
			using (var client = new HttpClient())
			{
				var responseMessage = await client.GetAsync(requestUrl).ConfigureAwait(false);
				return await ParseHttpResponse<T>(responseMessage);
			}
		}

		private static async Task<T> ParseHttpResponse<T>(HttpResponseMessage responseMessage) where T : class
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();

			if (!responseMessage.IsSuccessStatusCode)
			{
				var httpError = JsonConvert.DeserializeObject<HttpError>(responseContent);
				throw new Exception($"External API error occured. Status code: {httpError.Code}. Message: {httpError.Message}");
			}

			return JsonConvert.DeserializeObject<T>(responseContent);
		}
	}
}
