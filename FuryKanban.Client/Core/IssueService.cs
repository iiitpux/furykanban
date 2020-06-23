using FuryKanban.Shared.Model;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
    public class IssueService
    {
        public event EventHandler<AppState> OnStateChanged;
		
		private readonly AppHttpClient _httpClient;
		public IssueService(AppHttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IssueChangeResponse> InsertIssueAsync(AppState.Issue issue)
		{
			var result = await _httpClient.PostAsyncEx<IssueChangeResponse, AppState.Issue>("api/issue", issue, $"Insert issue '{issue.Title}'");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}

		public async Task<IssueChangeResponse> UpdateIssueAsync(AppState.Issue issue)
		{
			var result = await _httpClient.PutAsyncEx<IssueChangeResponse, AppState.Issue>("api/issue", issue, $"Update issue '{issue.Title}'");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}

		public async Task<IssueChangeResponse> DeleteIssueAsync(AppState.Issue issue)
		{
			var result = await _httpClient.DeleteAsyncEx<IssueChangeResponse>($"api/issue/{issue.Id}", $"Delete issue '{issue.Title}'");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}

		public async Task<IssueChangeResponse> ReorderIssueAsync(IssueReorder issueReorder)
		{
			var result = await _httpClient.PostAsyncEx<IssueChangeResponse, IssueReorder>("api/issue/reorder", issueReorder, $"Reorder issue");

			OnStateChanged?.Invoke(this, result.AppState);

			return result;
		}
	}
}