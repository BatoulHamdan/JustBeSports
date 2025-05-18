using JustBeSports.Core.Entities;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSportsAdmin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Properties

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductVariantService _productVariantService;
        private readonly IProductImageService _productImageService;

        #endregion

        #region Constructor

        public ProductController(IProductService productService, ICategoryService categoryService, IProductVariantService productVariantService, IProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productVariantService = productVariantService;
            _productImageService = productImageService;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Images(int id)
        {
            var images = _productImageService.GetProductImageByProductId(id);
            ViewBag.ProductId = id;
            return View(images);
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetAllProducts(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                return Json(_productService.GetProductsByCategoryId(categoryId.Value));
            }
            else
            {
                return Json(_productService.GetListOfProducts());
            }
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Save([FromBody] ProductModel product)
        {
            if (product == null)
                return BadRequest(new { message = "Invalid product data" });

            if (product.Id > 0)
            {
                // Update only the product’s main fields
                _productService.UpdateProduct(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Color,
                    product.CategoryId
                );

                return Ok(new { message = "Product updated successfully" });
            }
            else
            {
                var newProductId = _productService.AddProduct(
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Color,
                    product.CategoryId
                );

                return Ok(new
                {
                    message = "New product added successfully",
                    productId = newProductId
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] int ProductId, [FromForm] IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            var fileName = Path.GetFileName(ImageFile.FileName);

            var adminUploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets");
            if (!Directory.Exists(adminUploadsFolder))
            {
                Directory.CreateDirectory(adminUploadsFolder);
            }

            var baseDir = Directory.GetCurrentDirectory();
            var userUploadsFolder = Path.GetFullPath(Path.Combine(baseDir, "..", "JustBeWebsite", "wwwroot", "assets"));
            Console.WriteLine("User uploads folder full path: " + userUploadsFolder);


            if (!Directory.Exists(userUploadsFolder))
            {
                Directory.CreateDirectory(userUploadsFolder);
            }

            var adminFilePath = Path.Combine(adminUploadsFolder, fileName);
            using (var stream = new FileStream(adminFilePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            var userFilePath = Path.Combine(userUploadsFolder, fileName);
            using (var stream = new FileStream(userFilePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            _productImageService.InsertProductImage(fileName, ProductId);

            return Ok(new { message = "Image uploaded successfully." });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpDelete]
        public IActionResult DeleteImage(int id)
        {
            var image = _productImageService.GetProductImage(id);
            if (image == null)
                return NotFound(new { message = "Image not found" });

            var fileName = image.Url;
            var imagesWithSameFile = _productImageService.GetProductImagesByUrl(fileName)
                                .Where(img => img.Id != id)
                                .ToList();

            Console.WriteLine($"Images with same file excluding current id: {imagesWithSameFile.Count}");


            if (imagesWithSameFile == null)
            {
                var adminUploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets");
                var baseDir = Directory.GetCurrentDirectory();
                var userUploadsFolder = Path.GetFullPath(Path.Combine(baseDir, "..", "JustBeWebsite", "wwwroot", "assets"));

                var adminFilePath = Path.Combine(adminUploadsFolder, fileName);
                var userFilePath = Path.Combine(userUploadsFolder, fileName);

                Console.WriteLine($"Admin file path: {adminFilePath}");
                Console.WriteLine($"User file path: {userFilePath}");
                Console.WriteLine($"File exists in admin folder? {System.IO.File.Exists(adminFilePath)}");
                Console.WriteLine($"File exists in user folder? {System.IO.File.Exists(userFilePath)}");


                try
                {
                    if (System.IO.File.Exists(adminFilePath))
                    {
                        System.IO.File.Delete(adminFilePath);
                    }

                    if (System.IO.File.Exists(userFilePath))
                    {
                        System.IO.File.Delete(userFilePath);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Failed to delete image files: " + ex.Message });
                }
            }

            _productImageService.DeleteProductImage(id);

            return Ok(new { message = "Image deleted successfully" });
        }

        [HttpGet]
        public IActionResult GetAllVariants(int productId)
        {
            return Json(_productVariantService.GetProductVariantByProductId(productId));
        }

        [HttpPost]
        public IActionResult SaveVariant([FromBody] ProductVariantModel variant)
        {
            if (variant == null)
                return BadRequest(new { message = "Invalid variant data" });

            if (variant.Id > 0)
            {
                // update existing
                _productVariantService.UpdateProductVariant(
                    variant.Id,
                    variant.ProductId,
                    variant.Size,
                    variant.Stock
                );
                return Ok(new { message = "Variant updated successfully" });
            }
            else
            {
                // insert new
                _productVariantService.InsertProductVariant(
                    variant.ProductId,
                    variant.Size,
                    variant.Stock
                );
                return Ok(new { message = "Variant added successfully" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteVariant(int id)
        {
            _productVariantService.DeleteProductVariant(id);
            return Ok(new { message = "Variant deleted successfully" });
        }

        #endregion
    }
}
