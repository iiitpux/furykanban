using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuryKanban.Server.Contract;
using FuryKanban.Server.Filters;
using FuryKanban.Server.Logic;
using FuryKanban.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [TokenAuthorization]
    public class StageController : ControllerBase
    {
        private readonly IStageService _stageService;
        private readonly AuthUser _authUser;

        public StageController(IStageService stageService, AuthUser authUser)
        {
            _stageService = stageService;
            _authUser = authUser;
        }
		
        [HttpPost]
        [ServiceFilter(typeof(AppStateFilter))]
        public async Task<StageChangeResponse> Post(AppState.Stage stage)
        {
            return await _stageService.InsertOrUpdateAsync(stage, _authUser.Id);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AppStateFilter))]
        public async Task<StageChangeResponse> Delete(int id)
		{
			return await _stageService.DeleteAsync(id, _authUser.Id);
		}

		//private bool ProductsExists(int id)
		//{
		//    return _context.Products.Any(e => e.ProductId == id);
		//}
	}
}
