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
	public class IssueService
	{
		private AppDbContext _appDbContext;
		private ILogger<IssueService> _logger;

		public IssueService(AppDbContext appDbContext, ILogger<IssueService> logger)
		{
			_appDbContext = appDbContext;
			_logger = logger;
		}

		public async Task<IssueChangeResponse> InsertAsync(AppState.Issue issue, int userId)
		{
			var stage = await _appDbContext.Stages.FindAsync(issue.StageId);
			
			if (stage == null || (stage != null && stage.UserId != userId))
			{
				return new IssueChangeResponse()
				{
					HasError = true,
					ErrorMessage = "Issue not found"
				};
			}

			var issueDto = new IssueDto()
			{
				Body = issue.Body,
				CreatedDateTime = DateTime.Now,
				StageId = issue.StageId,
				Title = issue.Title,
				UserId = userId
			};

			_appDbContext.Issues.Add(issueDto);
			await _appDbContext.SaveChangesAsync();

			return new IssueChangeResponse();
		}

		public async Task<IssueChangeResponse> UpdateAsync(AppState.Issue issue, int userId)
		{
			var exist = await _appDbContext.Issues.Include(p=>p.Stage).SingleOrDefaultAsync(p=>p.Id == issue.Id);

			if(exist == null || exist.Stage == null || exist.Stage.UserId != userId)
				return new IssueChangeResponse()
				{
					HasError = true,
					ErrorMessage = "Issue not found"
				};

			exist.Title = issue.Title;
			exist.Body = issue.Body;
			await _appDbContext.SaveChangesAsync();

			return new IssueChangeResponse();
		}

		public async Task<IssueChangeResponse> DeleteAsync(int id, int userId)
		{
			var exist = await _appDbContext.Issues.Include(p => p.Stage).SingleOrDefaultAsync(p => p.Id == id);

			if (exist == null || exist.Stage == null || exist.Stage.UserId != userId)
				return new IssueChangeResponse()
				{
					HasError = true,
					ErrorMessage = "Issue not found"
				};

			_appDbContext.Issues.Remove(exist);
			await _appDbContext.SaveChangesAsync();

			return new IssueChangeResponse();
		}
	}
}
