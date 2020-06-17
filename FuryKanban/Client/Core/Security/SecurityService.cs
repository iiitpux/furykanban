using Blazored.LocalStorage;
using FuryKanban.Shared;
using FuryKanban.Shared.Model.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core.Security
{
	public class SecurityService : AuthenticationStateProvider
	{
		private readonly AppHttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public SecurityService(AppHttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _localStorage.GetItemAsync<string>(Const.Token);
			
			if (String.IsNullOrWhiteSpace(token))
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

			//todo- not work
			//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
			
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("token")));
		}

		public async Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registrationRequest)
		{
			var response = await _httpClient.PostAsJsonAsync("api/security/registration", registrationRequest);
			
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
				
				if (result.HasError)
					return result;

				await _localStorage.SetItemAsync<string>(Const.Token, result.Token);
				AuthenticationStateChange();

				return result;
			}

			return new RegistrationResponse()
			{
				ErrorMessage = response.StatusCode.ToString(),
				HasError = true
			};
		}

		public void Login()
		{
			throw new NotImplementedException();
		}

		public void LogOut()
		{
			throw new NotImplementedException();
		}

		public void AuthenticationStateChange()
		{
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
