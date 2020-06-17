using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuryKanban.Server.Logic;
using FuryKanban.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [TokenAuthorization]
    public class StageController : ControllerBase
    {
        private readonly StageService _stageService;
        private readonly AuthUser _authUser;

        public StageController(StageService stageService, AuthUser authUser)
        {
            _stageService = stageService;
            _authUser = authUser;
        }

        [HttpPost]
        public async Task<StageEditResponse> Post(AppState.Stage stage)
        {
            return await _stageService.InsertOrUpdateAsync(stage, _authUser.Id);
        }

        //// DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Products>> DeleteProducts(int id)
        //{
        //    var products = await _context.Products.FindAsync(id);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(products);
        //    await _context.SaveChangesAsync();

        //    return products;
        //}

        //private bool ProductsExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}
    }
}
