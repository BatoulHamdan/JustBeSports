using JustBeSports.Core.Entities;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;
using JustBeSports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSportsAdmin.Controllers
{
    public class CategoryController : AdminBaseController
    {
        #region Properties

        private readonly ICategoryService _categoryService;

        private readonly IProductService _productService;

        #endregion

        #region Constructors

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetListOfCategories();
            return Ok(categories);
        }


        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Save([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(new { message = "Invalid category data" });
            }

            if (category.Id > 0) // Edit existing category
            {
                _categoryService.UpdateCategory(category.Id, category.Name, category.Description);
                return Ok(new { message = "Category updated successfully" });
            }
            else // Add new category
            {
                _categoryService.AddCategory(category.Name, category.Description);
                return Ok(new { message = "New category added successfully" });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return Ok(new { message = "Category deleted successfully" });
        }

        #endregion
    }
}
