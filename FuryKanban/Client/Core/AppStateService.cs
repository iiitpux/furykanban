using System;
using System.Collections.Generic;
using FuryKanban.Shared.Model;

namespace FuryKanban.Client.Core
{
    public class AppStateService
    {
        private StageService _stageService;
        private IssueService _issueService;

        public List<AppState.Stage> Stages = new List<AppState.Stage>();

        public event EventHandler OnStateChange;

        public AppStateService(StageService stageService, IssueService issueService)
        {
            _stageService = stageService;
            _issueService = issueService;
            
            _stageService.OnStagesChange += StageServiceOnStagesChange;
        }

        private void StageServiceOnStagesChange(object sender, List<AppState.Stage> e)
        {
            //todo- update stages
            Stages = e;
            OnStateChange?.Invoke(null, new EventArgs());
        }
        //todo- unsubsribe        

        public void DeleteStage(int stageId)
		{
            //todo-
            throw new Exception();
		}
    }
}