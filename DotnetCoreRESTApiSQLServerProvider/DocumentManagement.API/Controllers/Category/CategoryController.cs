using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers
{
    /// <summary>
    /// Category
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        /// <param name="mediator"></param>
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get Specific Category by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var getCategoryQuery = new GetCategoryQuery
            {
                Id = id
            };
            var result = await _mediator.Send(getCategoryQuery);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get All Categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetCategories()
        {
            var getAllCategoryQuery = new GetAllCategoryQuery { IsParentOnly = true };
            var result = await _mediator.Send(getAllCategoryQuery);
            return Ok(result);
        }
        /// <summary>
        /// Create a Category.
        /// </summary>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand addCategoryCommand)
        {
            var result = await _mediator.Send(addCategoryCommand);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtAction("GetCategory", new { id = result.Id }, result);
        }
        /// <summary>
        /// Update Category.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateCategoryCommand"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
        public async Task<IActionResult> UpdateCategory(Guid Id, [FromBody] UpdateCategoryCommand category)
        {
            category.Id = Id;
            var result = await _mediator.Send(category);
            return StatusCode(result.StatusCode, result);

        }
        /// <summary>
        /// Delete Category.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var deleteCategoryCommand = new DeleteCategoryCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteCategoryCommand);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/subcategories")]
        [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetSubCategories(Guid id)
        {
            var getSubCategoriesQuery = new GetSubCategoriesQuery { Id = id };
            var result = await _mediator.Send(getSubCategoriesQuery);
            return Ok(result);
        }


        [HttpGet("dropdown")]
        [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetAllCategoriesForDropDown()
        {
            var getAllCategoryQuery = new GetAllCategoryQuery { IsParentOnly = false };
            var result = await _mediator.Send(getAllCategoryQuery);
            return Ok(result);
        }
    }
}
