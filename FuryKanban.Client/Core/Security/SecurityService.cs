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

			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("token")));
		}

		public async Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registrationRequest)
		{
			var response = await _httpClient.PostAsyncEx<RegistrationResponse, RegistrationRequest>("api/security/registration", registrationRequest, null);

			if (!response.HasError)
			{
				await _localStorage.SetItemAsync<string>(Const.Token, response.Token);
				AuthenticationStateChange();
			}

			return response;
		}

		public async Task<LoginResponse> Login(LoginRequest loginRequest)
		{
			var response = await _httpClient.PostAsyncEx<LoginResponse, LoginRequest>("api/security/login", loginRequest, null);

			if (!response.HasError)
			{
				await _localStorage.SetItemAsync<string>(Const.Token, response.Token);
				AuthenticationStateChange();
			}

			return response;
		}

		public async Task LogOut()
		{
			await _localStorage.RemoveItemAsync(Const.Token);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public void AuthenticationStateChange()
		{
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
