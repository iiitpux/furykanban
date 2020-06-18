using AutoMapper;
using FuryKanban.DataLayer;
using FuryKanban.DataLayer.Dto;
using FuryKanban.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Server.Logic
{
	public class AppService
	{
		private AppDbContext _appDbContext;
		public AppService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		//todo- history
		public async Task<AppState> GetStateAsync(int userId)
		{
			var stagesDto = await _appDbContext.Stages.Where(p => p.UserId == userId).Include(p=>p.Issues).ToListAsync();
			var config = new MapperConfiguration(cfg => cfg.CreateMap<StageDto, AppState.Stage>());
			var mapper = new Mapper(config);
			var stages = mapper.Map<List<AppState.Stage>>(stagesDto);
			return new AppState()
			{
				Stages = stages
			};
		}
	}
}
