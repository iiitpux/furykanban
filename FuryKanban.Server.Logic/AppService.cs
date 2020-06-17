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
			var res = await _appDbContext.Stages.Where(p => p.UserId == userId).Include(p=>p.Issues).ToListAsync();
			return new AppState();
		}
	}
}
