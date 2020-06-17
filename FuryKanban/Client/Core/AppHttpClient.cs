using Blazored.LocalStorage;
using FuryKanban.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace FuryKanban.Client.Core
{
	public class AppHttpClient : HttpClient
	{
		private ILocalStorageService _localStorageService;

		public AppHttpClient(ILocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;
		}

		public async Task<T> GetFromJsonAsyncWithToken<T>(string url)
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);

			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			return await this.GetFromJsonAsync<T>(url);
		}

		//public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		//{
		//	request.Headers.Add("token", "123");
		//	var token = await _localStorageService.GetItemAsync<string>("token");

		//	if(!String.IsNullOrWhiteSpace(token))
		//		request.Headers.Add("token", token);

		//	return await base.SendAsync(request, cancellationToken);
		//}

		public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
