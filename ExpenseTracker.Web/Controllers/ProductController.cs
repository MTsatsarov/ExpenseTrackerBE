using ExpenseTracker.Common;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/controller")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService productService;

		public ProductController(IProductService productService)
		{
			this.productService = productService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Get([FromQuery] string product)
		{
			var products = await this.productService.GetProducts(product);
			return this.Ok(products);
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		[Route("add")]
		public async Task<IActionResult> Create(ProductInputModel model)
		{
			var result = await this.productService.CreateProduct(model);

			if (!result)
			{
				return this.BadRequest();
			}
			return this.Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		[Route("delete")]
		public async Task<IActionResult> DeleteProduct(Guid productId)
		{
			var result = await this.productService.DeleteProduct(productId);

			if (!result)
			{
				return this.BadRequest("Unable to delete product");
			}

			return this.Ok(result);
		}
	}
}
