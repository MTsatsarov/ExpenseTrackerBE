using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService storageService;

        public StorageController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        [HttpGet]
        [Authorize]
        [Route("storage")]
        public async Task<IActionResult> GetStorage(int page = 0, int itemsPerPage = 10)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = await storageService.GetStorage(page,itemsPerPage,userId);

            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public async Task<IActionResult> AddStorage(StorageInputModel model)
        {
            model.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await storageService.Add(model);

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update(StorePatchModel model)
        {
            model.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await storageService.Update(model);

            return Ok();
        }

    }
}
