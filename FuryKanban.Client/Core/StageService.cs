using FuryKanban.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
	public class StageService
	{
		public event EventHandler<AppState> OnStateChanged;
		
		private readonly AppHttpClient _httpClient;
		public StageService(AppHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		//todo- общая часть для всех
		public async Task<StageChangeResponse> EditStageAsync(AppState.Stage stage)
		{
			var result = await _httpClient.PostAsyncEx<StageChangeResponse, AppState.Stage>("api/stage", stage, $"Edit column '{stage.Title}'");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}

		public async Task<StageChangeResponse> DeleteStageAsync(AppState.Stage stage)
		{
			var result = await _httpClient.DeleteAsyncEx<StageChangeResponse>($"api/stage/{stage.Id}", $"Delete column '{stage.Title}'");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}
	}
}
