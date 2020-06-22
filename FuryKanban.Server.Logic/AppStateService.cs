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
			var stagesDto = await _appDbContext.Stages.Where(p => p.UserId == userId).Include(p=>p.Issues).ToListAsync();
			var config = new MapperConfiguration(cfg => { 
				cfg.CreateMap<StageDto, AppState.Stage>();
				cfg.CreateMap<IssueDto, AppState.Issue>();
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

			return new AppState()
			{
				Stages = stages
			};
		}

		public async Task SetHistoryStateAsync(int userId, string title)
		{
			var state = await GetStateAsync(userId);
			_bufferHistory = new HistoryDto()
			{
				Body = JsonConvert.SerializeObject(state),
				Title = title,
				Committed = false,
				UserId = userId
			};
		}

		public async Task SaveHistoryStateAsync()
		{
			//todo- only 10 records
			_appDbContext.History.Add(_bufferHistory);
			await _appDbContext.SaveChangesAsync();
		}


	}
}
