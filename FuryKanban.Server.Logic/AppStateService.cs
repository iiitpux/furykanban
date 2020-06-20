using AutoMapper;
using FuryKanban.DataLayer;
using FuryKanban.DataLayer.Dto;
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
	public class AppStateService
	{
		private AppDbContext _appDbContext;
		private AppState _bufferState;
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
			return new AppState()
			{
				Stages = stages
			};
		}

		//public async Task<int> SaveHistoryState(int userId, string title)
		//{
		//	var state = await GetStateAsync(userId);
		//	var historyDto = new HistoryDto()
		//	{
		//		Body = JsonConvert.SerializeObject(state),
		//		Title = title,
		//		Committed = false
		//	};
		//}
	}
}
