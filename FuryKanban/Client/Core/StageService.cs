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
		public event EventHandler<List<AppState.Stage>> OnStagesChange;
		
		private readonly AppHttpClient _httpClient;
		public StageService(AppHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		//todo- общая часть для всех
		public async Task<StageEditResponse> EditStageAsync(AppState.Stage stage)
		{
			var response = await _httpClient.PostAsJsonAsync("api/stage", stage);

			if (!response.IsSuccessStatusCode)
				return new StageEditResponse()
				{
					ErrorMessage = response.StatusCode.ToString(),
					HasError = true
				};
			
			var result = await response.Content.ReadFromJsonAsync<StageEditResponse>();

			if (result.HasError)
				return result;

			OnStagesChange?.Invoke(this, result.Stages);
			
			return result;

		}
	}
}
