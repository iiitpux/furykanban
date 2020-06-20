using System;
using System.Collections.Generic;
using FuryKanban.Shared.Model;

namespace FuryKanban.Client.Core
{
	public class AppStateService : IDisposable
	{
		private StageService _stageService;
		private IssueService _issueService;

		public AppState State = new AppState()
		{
			Issues = new List<AppState.Issue>(),
			Stages = new List<AppState.Stage>()
		};

		public event EventHandler OnStateChanged;

		public AppStateService(StageService stageService, IssueService issueService)
		{
			_stageService = stageService;
			_issueService = issueService;

			_stageService.OnStateChanged += StateChanged;
			_issueService.OnStateChanged += StateChanged;
		}

		public void SetState(AppState state)
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