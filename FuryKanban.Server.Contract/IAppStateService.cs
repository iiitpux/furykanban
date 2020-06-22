using FuryKanban.Shared.Model;
using System;
using System.Threading.Tasks;

namespace FuryKanban.Server.Contract
{
	public interface IAppStateService
	{
		Task<AppState> GetStateAsync(int userId);

		Task SetHistoryStateAsync(int userId, string title);
		
		Task SaveHistoryStateAsync();
		Task<AppStateResponse> LoadHistory(int id, int userId);	}
}
