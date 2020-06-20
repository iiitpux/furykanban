using Blazored.LocalStorage;
using FuryKanban.Shared;
using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
	public class AppHttpClient : HttpClient, IHttpClient
	{
		public event EventHandler<IErrorResult> OnApiError;

		private ILocalStorageService _localStorageService;

		public AppHttpClient(ILocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;
		}

		public async Task<TResult> GetAsyncEx<TResult>(string url) where TResult : IErrorResult, new()
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);

			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			return await this.GetFromJsonAsync<TResult>(url);
		}

		public async Task<TResult> PostAsyncEx<TResult, TValue>(string url, TValue model, string actionName) where TResult : IErrorResult, new()
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.PostAsJsonAsync<TValue>(url, model);

			if (!response.IsSuccessStatusCode)
			{
				var errorResult = new TResult()
				{
					HasError = true,
					ErrorMessage = response.StatusCode.ToString()
				};
				OnApiError?.Invoke(this, errorResult);

				return errorResult;
			}

			var result =  await response.Content.ReadFromJsonAsync<TResult>();

			if (!result.HasError)
				return result;

			OnApiError?.Invoke(this, result);

			return result;
		}

		public async Task<TResult> PutAsyncEx<TResult, TValue>(string url, TValue model, string actionName) where TResult : IErrorResult, new()
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.PutAsJsonAsync<TValue>(url, model);

			if (!response.IsSuccessStatusCode)
			{
				var errorResult = new TResult()
				{
					HasError = true,
					ErrorMessage = response.StatusCode.ToString()
				};
				OnApiError?.Invoke(this, errorResult);

				return errorResult;
			}

			var result = await response.Content.ReadFromJsonAsync<TResult>();

			if (!result.HasError)
				return result;

			OnApiError?.Invoke(this, result);

			return result;
		}

		public async Task<TResult> DeleteAsyncEx<TResult>(string url, string actionName) where TResult : IErrorResult, new()
		{
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.DeleteAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				var errorResult = new TResult()
				{
					HasError = true,
					ErrorMessage = response.StatusCode.ToString()
				};
				OnApiError?.Invoke(this, errorResult);

				return errorResult;
			}

			var result = await response.Content.ReadFromJsonAsync<TResult>();

			if (!result.HasError)
				return result;

			OnApiError?.Invoke(this, result);

			return result;
		}

		//public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		//{
		//	var token = await _localStorageService.GetItemAsync<string>(Const.Token);
		//	if (!String.IsNullOrWhiteSpace(token))
		//	{
		//		this.DefaultRequestHeaders.Remove(Const.Token);
		//		this.DefaultRequestHeaders.Add(Const.Token, token);
		//	}

		//	return await base.SendAsync(request, cancellationToken);
		//}
	}
}
