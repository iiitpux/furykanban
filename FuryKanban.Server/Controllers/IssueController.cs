using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuryKanban.Server.Contract;
using FuryKanban.Server.Filters;
using FuryKanban.Server.Logic;
using FuryKanban.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [TokenAuthorization]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly AuthUser _authUser;

        public IssueController(IIssueService issueService, AuthUser authUser)
        {
            _issueService = issueService;
            _authUser = authUser;
        }

		[HttpPost]
		[ServiceFilter(typeof(AppStateFilter))]
		public async Task<IssueChangeResponse> Post(AppState.Issue issue)
		{
			return await _issueService.InsertAsync(issue, _authUser.Id);
		}

        [HttpPut]
        [ServiceFilter(typeof(AppStateFilter))]
        public async Task<IssueChangeResponse> Put(AppState.Issue issue)
        {
            return await _issueService.UpdateAsync(issue, _authUser.Id);
        }

		[HttpDelete("{id}")]
		[ServiceFilter(typeof(AppStateFilter))]
		public async Task<IssueChangeResponse> Delete(int id)
		{
			return await _issueService.DeleteAsync(id, _authUser.Id);
		}

		[HttpPost(template:"reorder")]
		[ServiceFilter(typeof(AppStateFilter))]
		public async Task<IssueChangeResponse> Reorder(IssueReorder issueReorder)
		{
			return await _issueService.ReorderAsync(issueReorder, _authUser.Id);
		}
	}
}
