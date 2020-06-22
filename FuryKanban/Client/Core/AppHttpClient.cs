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
		private LoaderService _loaderService;

		public AppHttpClient(ILocalStorageService localStorageService, LoaderService loaderService)
		{
			_localStorageService = localStorageService;
			_loaderService = loaderService;
		}

		public async Task<TResult> GetAsyncEx<TResult>(string url) where TResult : IErrorResult, new()
		{
			await _loaderService.LoadStart();
			var token = await _localStorageService.GetItemAsync<string>(Const.Token);

			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var result = await this.GetFromJsonAsync<TResult>(url);

			await _loaderService.LoadEnd();
			
			return result;
		}

		public async Task<TResult> PostAsyncEx<TResult, TValue>(string url, TValue model, string actionName) where TResult : IErrorResult, new()
		{
			await _loaderService.LoadStart();

			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.PostAsJsonAsync<TValue>(url, model);

			await _loaderService.LoadEnd();

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
			await _loaderService.LoadStart();

			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.PutAsJsonAsync<TValue>(url, model);

			await _loaderService.LoadEnd();

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
			await _loaderService.LoadStart();

			var token = await _localStorageService.GetItemAsync<string>(Const.Token);
			if (!String.IsNullOrWhiteSpace(token))
			{
				this.DefaultRequestHeaders.Remove(Const.Token);
				this.DefaultRequestHeaders.Add(Const.Token, token);
			}

			var response = await this.DeleteAsync(url);

			await _loaderService.LoadEnd();

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
	}
}
