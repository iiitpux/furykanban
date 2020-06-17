using AutoMapper;
using FuryKanban.DataLayer;
using FuryKanban.DataLayer.Dto;
using FuryKanban.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Server.Logic
{
	//todo- to interface
	public class StageService
	{
		private AppDbContext _appDbContext;
		private ILogger<StageService> _logger;

		public StageService(AppDbContext appDbContext, ILogger<StageService> logger)
		{
			_appDbContext = appDbContext;
			_logger = logger;
		}

		//public async Task<List<AppState.Stage>> GetStagesAsync(int userId)
		//{
		//	return await _appDbContext.Stages.Where(p => p.UserId == userId).Select(p=> new AppState.Stage() { 
		//		Id = p.Id,

		//	}).ToListAsync();
		//}

		public async Task<StageEditResponse> InsertOrUpdateAsync(AppState.Stage stage, int userId)
		{
			async Task AddNewStage()
			{
				var stageDto = new StageDto()
				{
					UserId = userId,
					Order = stage.Order,
					Title = stage.Title
				};
				_appDbContext.Stages.Add(stageDto);
				await _appDbContext.SaveChangesAsync();
			}

			if (stage.Id != 0)
			{
				var exist = await _appDbContext.Stages.SingleOrDefaultAsync(p => p.Id == stage.Id && p.UserId == userId);
				if (exist != null)
				{
					exist.Title = stage.Title;
					exist.Order = stage.Order;
					await _appDbContext.SaveChangesAsync();
				}
				else
					await AddNewStage();
			}
			else
				await AddNewStage();

			var config = new MapperConfiguration(cfg => cfg.CreateMap<StageDto, AppState.Stage>());
			var mapper = new Mapper(config);
			var stagesDto = _appDbContext.Stages.AsNoTracking().Where(p => p.UserId == userId).ToList();
			var stages = mapper.Map<List<AppState.Stage>>(stagesDto);

			return new StageEditResponse()
			{
				Stages = stages
			};
		}
	}
}
