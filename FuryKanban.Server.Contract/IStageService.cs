using FuryKanban.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Server.Contract
{
	public interface IStageService
	{
		Task<StageChangeResponse> InsertOrUpdateAsync(AppState.Stage stage, int userId);

		Task<StageChangeResponse> DeleteAsync(int id, int userId);
	}
}
