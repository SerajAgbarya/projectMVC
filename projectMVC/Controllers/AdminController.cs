using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectMVC.DataTypes;
using projectMVC.Models;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace projectMVC.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly OnlineShopDbContext _dbContext;

        public AdminController(OnlineShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("user/create")]
        public async Task<IActionResult> CreateUserAction([FromForm] CreateUserDTO inputUser)
        {
            var user = new UserModel
            {
                UserName = inputUser.UserName,
                pasWord = inputUser.PasWord,
                PhoneNumber = inputUser.PhoneNumber,
                Email = inputUser.Email,
                UserAddress = inputUser.UserAddress
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();


            var userPermission = new UserPermissionModel
            {
                PermissionType = inputUser.UserType.ToString(),
                UserId = user.Id   
            };

            _dbContext.Users_permission.Add(userPermission);
            await _dbContext.SaveChangesAsync();


            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, inputUser);
        }


        [HttpGet("user/create")]
        public IActionResult CreateUser()
        {
            return View("CreateUser"
                );
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO
            {
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserAddress = user.UserAddress
            };

            return View(userDto); // Return the view with the user DTO model
        }


        [HttpGet("ref")]
        public IActionResult CreatRefData()
        {
            return View("creatRefData");
        }


        [HttpGet("category/create")]
        public IActionResult CreatCategory()
        {
            return View("creatCategory");
        }


        [HttpPost("category/create")]
        public async Task<IActionResult> CreatCategoryAction([FromForm] CreatCategoryDTO input)
        {
            var category = new ProductCategoresModel
            {
                CategoryName = input.CategoryName,
            };

            _dbContext.Product_categories.Add(category);
            await _dbContext.SaveChangesAsync();

            foreach (var attributeName in input.AttributeNames)
            {
                var categoryAttribute = new ProductCategoryAttributesModel
                {
                    AttributeName =  attributeName,
                    CategoryName = category.CategoryName
                };

                _dbContext.Product_category_attributes.Add(categoryAttribute);
            }

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { CategoryName = category.CategoryName }, category);
        }



        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<CreatCategoryDTO>> GetCategory(string categoryName)
        {
            var category = await _dbContext.Product_categories.FindAsync(categoryName);

            if (category == null)
            {
                return NotFound(); // Return 404 Not Found if the category doesn't exist
            }

            var createCategoryDTO = new CreatCategoryDTO
            {
                CategoryName = category.CategoryName,
                // You can add more properties to the DTO if needed
            };

            return createCategoryDTO;
        }



        [HttpGet("product/create")]
        public async Task<IActionResult> CreateProduct()
        {
            Dictionary<string, List<string>> categoryAttributesDict = await Utils.CategoriesWithAttributesDict(_dbContext);


            // Create a new CreateProductDTO object to pass to the view
            var createProductDTO = new CreateProductDTO
            {
                categoryAttributesDict = categoryAttributesDict
            };

            return View("createProduct", createProductDTO);
        }

      

        [HttpPost("product/create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO input)
        {
            try
            {
                // Create ProductsModel object
                var product = new ProductsModel
                {
                    Category = input.Category,
                    ImageUrl = input.ImageUrl,
                    StockQuantity = input.StockQuantity,
                    Price = input.Price,
                    StockLocation = input.StockLocation,
                    brand = input.brand
                };

                // Add product to database
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                // Create ProductAttributesModel objects for dynamic attributes
                if (input.Attributes != null)
                {
                    foreach (var attribute in input.Attributes)
                    {
                        var productAttribute = new ProductAttributesModel
                        {
                            ProductId = product.Id,
                            AttributeName = attribute.name,
                            AttributeValue = attribute.value
                        };
                        _dbContext.Add(productAttribute);
                    }
                    await _dbContext.SaveChangesAsync();
                }

                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the product: {ex.Message}");
            }
        }
    }
}




