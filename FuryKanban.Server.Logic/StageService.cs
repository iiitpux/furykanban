using AutoMapper;
using FuryKanban.DataLayer;
using FuryKanban.DataLayer.Dto;
using FuryKanban.Server.Contract;
using FuryKanban.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Server.Logic
{
	public class StageService : IStageService
	{
		private AppDbContext _appDbContext;

		public StageService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<StageChangeResponse> InsertOrUpdateAsync(AppState.Stage stage, int userId)
		{
			async Task AddNewStage()
			{
				var stageDto = new StageDto()
				{
					UserId = userId,
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
					await _appDbContext.SaveChangesAsync();
				}
				else
					await AddNewStage();
			}
			else
				await AddNewStage();

			return new StageChangeResponse();
		}

		public async Task<StageChangeResponse> DeleteAsync(int id, int userId)
		{
			var stage = await _appDbContext.Stages.FindAsync(id);

			if (stage == null || stage.UserId != userId)
			{
				return new StageChangeResponse() { 
					ErrorMessage = "Stage not found"
				};
			}

			_appDbContext.Stages.Remove(stage);
			await _appDbContext.SaveChangesAsync();

			return new StageChangeResponse();
		}
	}
}
