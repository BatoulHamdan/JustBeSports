using JustBeSports.Core.Entities;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;
using JustBeSports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSportsAdmin.Controllers
{
    public class CategoryController : Controller
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

        #endregion
    }
}
