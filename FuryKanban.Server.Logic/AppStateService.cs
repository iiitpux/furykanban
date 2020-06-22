using AutoMapper;
using FuryKanban.DataLayer;
using FuryKanban.DataLayer.Dto;
using FuryKanban.Server.Contract;
using FuryKanban.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Server.Logic
{
	public class AppStateService : IAppStateService
	{
		private AppDbContext _appDbContext;
		private HistoryDto _bufferHistory;
		public AppStateService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		//todo- history
		public async Task<AppState> GetStateAsync(int userId)
		{
			var stagesDto = await _appDbContext.Stages.Where(p => p.UserId == userId).Include(p => p.Issues).ToListAsync();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<StageDto, AppState.Stage>();
				cfg.CreateMap<IssueDto, AppState.Issue>();
				cfg.CreateMap<HistoryDto, AppState.History>();
			});
			var mapper = new Mapper(config);
			var stages = mapper.Map<List<AppState.Stage>>(stagesDto);

			var allIssues = stages.SelectMany(p => p.Issues).ToList();

			foreach (var stage in stagesDto)
			{
				var lastIssue = stage.Issues.SingleOrDefault(p => !p.NextIssueId.HasValue);
				var backIndex = 0;
				while (lastIssue != null)
				{
					var iss = allIssues.Single(p => p.Id == lastIssue.Id);
					iss.Order = stage.Issues.Count - backIndex;
					backIndex++;
					lastIssue = stage.Issues.SingleOrDefault(p => p.NextIssueId == lastIssue.Id);
				}
			}

			var undoList = mapper.Map<List<AppState.History>>(await _appDbContext.History.Where(p => p.UserId == userId).ToListAsync());

			return new AppState()
			{
				Stages = stages,
				UndoList = undoList
			};
		}

		public async Task SetHistoryStateAsync(int userId, string title)
		{
			var state = await _appDbContext.Stages.AsNoTracking().Where(p => p.UserId == userId).Include(p => p.Issues).ToListAsync();

			foreach (var stage in state)
			{
				stage.User = null;
				foreach (var issue in stage.Issues)
				{
					issue.Stage = null;
					issue.NextIssue = null;
					issue.User = null;
				}
			}

			_bufferHistory = new HistoryDto()
			{
				Body = JsonConvert.SerializeObject(state),
				Title = title,
				UserId = userId
			};
		}

		public async Task SaveHistoryStateAsync()
		{
			if (_bufferHistory == null)
				return;

			var toRemove = _appDbContext.History
				.Where(p => p.UserId == _bufferHistory.UserId)
				.OrderByDescending(p => p.Id)
				.Skip(9);
			_appDbContext.History.RemoveRange(toRemove);

			_appDbContext.History.Add(_bufferHistory);
			await _appDbContext.SaveChangesAsync();
		}

		public async Task<AppStateResponse> LoadHistory(int id, int userId)
		{
			var history = await _appDbContext.History.FindAsync(id);
			if (history == null || history.UserId != userId)
				return new AppStateResponse()
				{
					HasError = true,
					ErrorMessage = "History not found"
				};

			var historyToRemove = _appDbContext.History.Where(p => p.UserId == userId && p.Id >= history.Id);
			_appDbContext.History.RemoveRange(historyToRemove);

			var stagesToRemove = _appDbContext.Stages.Where(p => p.UserId == userId);
			_appDbContext.Stages.RemoveRange(stagesToRemove);

			var issuesToRemove = _appDbContext.Issues.Where(p => stagesToRemove.SelectMany(i => i.Issues).Select(z => z.Id).Contains(p.Id));
			_appDbContext.Issues.RemoveRange(issuesToRemove);

			var stages = JsonConvert.DeserializeObject<List<StageDto>>(history.Body);

			foreach (var stage in stages.OrderBy(p=>p.Id))
			{
				var newStage = new StageDto() { 
					Title = stage.Title,
					UserId = stage.UserId,
					Issues = new List<IssueDto>()
				};
				//todo- nextissueid
				foreach (var issue in stage.Issues)
				{
					newStage.Issues.Add(new IssueDto() {
						Id = issue.Id,
						Body = issue.Body,
						CreatedDateTime = issue.CreatedDateTime,
						StageId = issue.StageId,
						Title = issue.Title,
						UserId = issue.UserId,
						NextIssueId = issue.NextIssueId
					});
				}
				_appDbContext.Stages.Add(newStage);
			}

			await _appDbContext.SaveChangesAsync();

			return new AppStateResponse() { AppState = await GetStateAsync(userId) };
		}
	}
}
