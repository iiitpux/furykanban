using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FuryKanban.Client
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        //private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, IJSRuntime jsRuntime)//, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            //_localStorage = localStorage;
            //state.StateChanged += OnStateChanged;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("getCookie", "token");
            if(token != null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("token")));

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            //var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            //if (string.IsNullOrWhiteSpace(savedToken))
            //{
            //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //}

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        public void MarkUserAsAuthenticated()
        {
            //throw new Exception("fu");
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("token"))));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            //throw new Exception("fu");
            //var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
			NotifyAuthenticationStateChanged(authState);
		}
    }
}
