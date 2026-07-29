using Inventory.API.DTOs.Category;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //throw new Exception("Testing Global Exception");
            var categories = await _categoryService.GetAllAsync();

            //return Ok(categories);
            //return Ok(
            //            new ApiResponse<IEnumerable<CategoryDto>>
            //            (
            //                true,
            //                "Categories fetched successfully.",
            //                categories
            //            ));
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories,"Success"));
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);

        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto);

            if (!result)
                return BadRequest(ApiResponse<object>.FailureResponse("Category already exists",null));
            //return BadRequest(
            //    new ApiResponse<object>
            //    (
            //        false,
            //        "Category already exists.",
            //        null
            //    ));
            //return BadRequest("Category already exists.");

            //return Ok("Category created successfully.");
            return Ok(ApiResponse<object>.SuccessResponse(result, "Category created successfully"));
                       
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Manager")]
        [Authorize(Roles =Roles.Admin+","+Roles.Manager)]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<Object>.FailureResponse("Id mismatch.", null));
                    //new ApiResponse<object>
                    //(
                    //    false,
                    //    "Id mismatch.",
                    //    null
                    //));
            //return BadRequest("Id mismatch.");

            var result = await _categoryService.UpdateAsync(dto);

            if (!result)
                return NotFound(ApiResponse<object>.FailureResponse("Category deleted successfully.",null));
            //return Ok(
            //     new ApiResponse<object>
            //     (
            //         true,
            //         "Category deleted successfully.",
            //         NotFound()
            //     ));

            //return Ok("Category updated successfully.");
            return Ok(ApiResponse<object>.SuccessResponse(result, "Category Updated successfully."));

        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result)
                return NotFound(ApiResponse<object>.FailureResponse("Records not found",null));

            //return Ok("Category deleted successfully.");
            return Ok(ApiResponse<object>.SuccessResponse(result, "Category deleted successfully."));
        }
    }
}