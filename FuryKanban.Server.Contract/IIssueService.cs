using FuryKanban.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Server.Contract
{
	public interface IIssueService
	{
		Task<IssueChangeResponse> InsertAsync(AppState.Issue issue, int userId);
		Task<IssueChangeResponse> UpdateAsync(AppState.Issue issue, int userId);
		Task<IssueChangeResponse> DeleteAsync(int id, int userId);
		Task<IssueChangeResponse> ReorderAsync(IssueReorder issueReorder, int userId);
	}
}
