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
	//todo- logger?
	public class IssueService : IIssueService
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

			var allNextIds = await _appDbContext.Issues.Where(p => p.StageId == stage.Id
					&& p.NextIssueId.HasValue)
					.Select(p => p.NextIssueId.Value).ToListAsync();
			var first = await _appDbContext.Issues.SingleOrDefaultAsync(p => !allNextIds.Contains(p.Id)
				&& p.StageId == stage.Id);

			int? nextIssueId = null;
			if (first != null)
				nextIssueId = first.Id;

			var issueDto = new IssueDto()
			{
				Body = issue.Body,
				CreatedDateTime = DateTime.Now,
				StageId = issue.StageId,
				Title = issue.Title,
				UserId = userId,
				NextIssueId = nextIssueId
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

		public async Task<IssueChangeResponse> ReorderAsync(IssueReorder issueReorder, int userId)
		{
			if (issueReorder.Id == issueReorder.TargetId)
				return new IssueChangeResponse();

			//remove from old position
			var exist = await _appDbContext.Issues.Include(p => p.Stage).SingleOrDefaultAsync(p => p.Id == issueReorder.Id);

			if (exist == null || exist.Stage == null || exist.Stage.UserId != userId)
				return new IssueChangeResponse()
				{
					HasError = true,
					ErrorMessage = "Issue not found"
				};

			if (exist.StageId != issueReorder.NewStageId)
			{
				var existStage = await _appDbContext.Stages.FindAsync(issueReorder.NewStageId);
				if (existStage != null && existStage.UserId != userId)
					return new IssueChangeResponse()
					{
						HasError = true,
						ErrorMessage = "Issue not found"
					};
			}

			var prevIssue = await _appDbContext.Issues.SingleOrDefaultAsync(p => p.NextIssueId == issueReorder.Id);
			if (prevIssue != null)
				prevIssue.NextIssueId = exist.NextIssueId;

			//insert to new position
			exist.StageId = issueReorder.NewStageId;

			if (issueReorder.TargetId == 0)
			{
				var allNextIds = await _appDbContext.Issues.Where(p => p.StageId == issueReorder.NewStageId 
					&& p.NextIssueId.HasValue)
					.Select(p => p.NextIssueId.Value).ToListAsync();
				var first = await _appDbContext.Issues.SingleOrDefaultAsync(p => !allNextIds.Contains(p.Id)
					&& p.StageId == issueReorder.NewStageId);
				if(first != null)
					exist.NextIssueId = first.Id;
			}
			else
			{
				var targetIssue = await _appDbContext.Issues.Include(p => p.Stage).SingleOrDefaultAsync(p => p.Id == issueReorder.TargetId);
				if (targetIssue == null || targetIssue.Stage == null || targetIssue.Stage.UserId != userId)
					return new IssueChangeResponse()
					{
						HasError = true,
						ErrorMessage = "Issue not found"
					};

				exist.NextIssueId = targetIssue.NextIssueId;
				targetIssue.NextIssueId = exist.Id;
			}

			await _appDbContext.SaveChangesAsync();

			return new IssueChangeResponse();
		}
	}
}
