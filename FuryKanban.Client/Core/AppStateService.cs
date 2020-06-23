using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuryKanban.Shared.Model;

namespace FuryKanban.Client.Core
{
	public class AppStateService : IDisposable
	{
		private StageService _stageService;
		private IssueService _issueService;
		private AppHttpClient _httpClient;

		public AppState State = new AppState()
		{
			Stages = new List<AppState.Stage>(),
			UndoList = new List<AppState.History>(),
		};

		public event EventHandler OnStateChanged;

		public AppStateService(StageService stageService, IssueService issueService, AppHttpClient httpClient)
		{
			_stageService = stageService;
			_issueService = issueService;
			_httpClient = httpClient;

			_stageService.OnStateChanged += StateChanged;
			_issueService.OnStateChanged += StateChanged;
		}

		public async Task LoadStateAsync()
		{
			var appState = await _httpClient.GetAsyncEx<FuryKanban.Shared.Model.AppStateResponse>("api/app");
			SetState(appState.AppState);
		}

		public async Task LoadHistoryAsync(int id)
		{
			var appState = await _httpClient.GetAsyncEx<FuryKanban.Shared.Model.AppStateResponse>("api/app/history/" + id);
			SetState(appState.AppState);
		}

		private void SetState(AppState state)
		{
			if (state == null)
				return;

			State = state;

			OnStateChanged?.Invoke(null, new EventArgs());
		}

		private void StateChanged(object sender, AppState appState)
		{
			SetState(appState);
		}

		public void Dispose()
		{
			_stageService.OnStateChanged -= StateChanged;
			_issueService.OnStateChanged -= StateChanged;
		}
	}
}