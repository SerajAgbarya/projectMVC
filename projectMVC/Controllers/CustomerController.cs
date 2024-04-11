using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projectMVC.Models;
using projectMVC.DataTypes;
using projectMVC.DataTypes.Customer;
using projectMVC.enums;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

namespace projectMVC.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly OnlineShopDbContext _dbContext;

        public CustomerController(OnlineShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("signUp")]
        public IActionResult signUp()
        {
            return View("signUp");
        }

        [HttpPost("signUpForm")]
        public async Task<IActionResult> signUpAction([FromForm] UserDTO inputUser)
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
                PermissionType = "Customer",
                UserId = user.Id
            };

            _dbContext.Users_permission.Add(userPermission);
            await _dbContext.SaveChangesAsync();


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string token = null, string category = null, string brand = null, int? quantity = null)
        {
            // Start with retrieving all products from the database
            IQueryable<ProductsModel> query = _dbContext.Products;
            int userId = getGuestUserId();
            if (token != null)
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

                // Access claims from the token
                Claim userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    userId = int.Parse(userIdClaim.Value);
                }
               

            }

            // Apply filters based on the parameters provided
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.brand == brand);
            }

            if (quantity.HasValue)
            {
                query = query.Where(p => p.StockQuantity > quantity);
            }


            Console.WriteLine("query" + query);
            // Execute the query to retrieve products matching the filters
            var productsModel = await query.ToListAsync();

            // Map products and their attributes to DTOs
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            foreach (var productModel in productsModel)
            {
                ProductDTO productDTO = await mapProductModelToDTO(productModel);

                productDTOList.Add(productDTO);
            }

            Dictionary<string, List<string>> categoryAttributesDict = await Utils.CategoriesWithAttributesDict(_dbContext);

            var allProductsDTO = new AllProductsDTO
            {
                products = productDTOList,
                categoryAttributesDict = categoryAttributesDict
            };
            
        

            List<CartModel> cartProductsList = _dbContext.Cart.Where(c => c.UserId == userId).ToList();

            var cartProductDTOList = new List<ProductDTO>();
            foreach (var entity in cartProductsList)
            {
                ProductsModel p =  _dbContext.Products.FirstOrDefault(c => c.Id == entity.ProductId);
                ProductDTO pDto = await mapProductModelToDTO(p);
                cartProductDTOList.Add(pDto);
            }
            allProductsDTO.Cart = new CartDTO
            {
                products = cartProductDTOList
            };
           
            // Send DTOs to the view
            return View("ProductsView", allProductsDTO);
        }

        private int getGuestUserId()
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserName == "Guest").Id;
        }

        [HttpPost("initiateOrder")]
        public async Task<IActionResult> initiateOrder([FromBody] InitiateOrderDTO input)
        {
          
            // Find the claim representing the user ID
            string userId = getUserFromContext();

            OrderModel orderModel = new OrderModel
            {
                OrderStatus = OrderStatus.CREATED.ToString(),
                UserId = int.Parse(userId),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _dbContext.Orders.Add(orderModel);

            await _dbContext.SaveChangesAsync();

            foreach (int ProductId in input.ProductIds)
            {
                var productModel = _dbContext.Products.Find(ProductId);

                OrderProductsModel orderProductsModel = new OrderProductsModel
                {
                    orderId = orderModel.Id,
                    productId = productModel.Id
                };
                _dbContext.Order_products.Add(orderProductsModel);
            }

            await _dbContext.SaveChangesAsync();
            return Json(new { orderId = orderModel.Id });
        }

        private string getUserFromContext()
        {
            // Get the current user's claims
            ClaimsPrincipal currentUser = HttpContext.User;

            string userId;
            Claim userIdClaim = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                userId = userIdClaim.Value;
            }
            else
            {
                userId = _dbContext.Users.FirstOrDefault(u => u.UserName == "Guest").Id.ToString();
            }

            return userId;
        }

        //[OrderAuthorize("OnlineShopDbContext")]
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> CreateOrder(int orderId)
        {
            CreateOrderDTO orderDTO = new CreateOrderDTO();

            OrderModel order = _dbContext.Orders.First(entity => entity.Id == orderId);
            var orderProductModels = _dbContext.Order_products.Where(entity => entity.orderId == orderId).ToList();

            orderDTO.cartDTO = new CartDTO();

            foreach ( var orderProductPair in orderProductModels)
            {
                var productModel = _dbContext.Products.First(p => p.Id == orderProductPair.productId);
                ProductDTO productDTO = await mapProductModelToDTO(productModel);
                DiscountModel? latestDiscount = getDiscount(productDTO.Id);
                if (latestDiscount != null)
                {
                    productDTO.discountPrice = productDTO.Price - (productDTO.Price * latestDiscount.Percentage / 100);
                }
                else
                {
                    productDTO.discountPrice = productDTO.Price;
                }
                orderDTO.cartDTO.products = new List<ProductDTO>();
                orderDTO.cartDTO.products.Add(productDTO);
            }

            return View("CreateOrder" , orderDTO);
        }

        [HttpPost("Order/{orderId}")]
        public ActionResult submitOrder(int orderId, [FromBody] CreateOrderDTO input)
        {
            foreach (var productDto in input.cartDTO.products)
            {
                var productModel = _dbContext.Products.Find(productDto.Id);
                if (productModel.StockQuantity < productDto.requiredQuntity)
                {
                    //THROW eroro and update orderSTatus=failed??
                }
            }

            if (input.isPickupOrder || validateShippingAddress(input.shippingAddress))
            {
                IQueryable<ProductsModel> query = _dbContext.Products;
                OrderModel order = _dbContext.Orders.First(entity => entity.Id == orderId);
               
                order.IsPickupOrder = input.isPickupOrder;
                   
                List<OrderProductsModel> orderProductModels = _dbContext.Order_products.Where(entity => entity.orderId == orderId).ToList();
                foreach ( var productDto in input.cartDTO.products)
                {
                    OrderProductsModel model = _dbContext.Order_products.First(op => op.orderId == orderId && op.productId == productDto.Id);
                    var productModel = _dbContext.Products.Find(productDto.Id);
                    DiscountModel? latestDiscount = getDiscount(productDto.Id);
                    if (latestDiscount != null)
                    {
                        model.actaulPrice = productModel.Price - (productModel.Price * latestDiscount.Percentage / 100);
                    }
                    else { 
                        model.actaulPrice = productModel.Price;
                    }
                    model.quanity = productDto.requiredQuntity;
                     
                    productModel.StockQuantity -= productDto.requiredQuntity.Value;
                      
                }

                if (ProcessPayment(input.payment))
                {
                    order.OrderStatus = OrderStatus.CONFIRMED.ToString();
                    _dbContext.SaveChanges();
                    return Content("Payment successful!");
                }
                else
                {
                  
                    return Content("Payment failed. Please try again.");
                }
            }
            else
            {
                return Content("invalid Addess, Please try again.");
            } 
         
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddToCartAction([FromBody] AddToCartDTO input)
        {
            if (input == null || input.ProductIds == null || input.ProductIds.Count == 0)
            {
                return BadRequest("No product IDs provided.");
            }

            try
            {
                int userId = int.Parse(getUserFromContext());

                foreach (int productId in input.ProductIds)
                {
                    var productModel = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
                    if (productModel == null)
                    {
                        return BadRequest($"Product with ID {productId} not found.");
                    }

                    decimal actualPrice = productModel.Price;
                    DiscountModel? latestDiscount = getDiscount(productId);
                    if (latestDiscount != null)
                    {
                        actualPrice -= (productModel.Price * latestDiscount.Percentage / 100);
                    }

                    var existingCartItem = _dbContext.Cart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
                    if (existingCartItem != null)
                    {
                        // If the product already exists in the cart, increase its quantity
                        existingCartItem.Quantity++;
                    }
                    else
                    {
                        // If the product does not exist in the cart, add a new cart item
                        var cartItem = new CartModel
                        {
                            UserId = userId,
                            ProductId = productId,
                            Quantity = 1,
                            ActualPrice = actualPrice
                        };

                        _dbContext.Cart.Add(cartItem);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok("Products added to cart successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        private bool ProcessPayment(PaymentDTO input)
        {
            //TODO we may call exeranl gateways/sysem to valiidate if such CC is valid and have creidt or call Paypal from here
            return true;
        }

        private bool validateShippingAddress(ShippingAddressDTO input)
        {

            //TODO we may call exeranl gateways/sysem to valiidate address
            return true;
        }

        private async Task<ProductDTO> mapProductModelToDTO(ProductsModel? productModel)
        {
            var attributes = await _dbContext.Product_attributes
                                .Where(attr => attr.ProductId == productModel.Id)
                                .ToDictionaryAsync(attr => attr.AttributeName, attr => attr.AttributeValue);

            var productDTO = new ProductDTO
            {
                Id = productModel.Id,
                Category = productModel.Category,
                ImageUrl = productModel.ImageUrl,
                StockLocation = productModel.StockLocation,
                StockQuantity = productModel.StockQuantity,
                Price = productModel.Price,
                brand = productModel.brand,
                AttributesDict = attributes
            };
            return productDTO;
        }

        private DiscountModel? getDiscount(int? productId)
        {
            DateTime currentDate = DateTime.Now;
            return _dbContext.Discounts
                .Where(d => d.ProductId == productId && (!d.StartDate.HasValue || d.StartDate <= currentDate) && (!d.EndDate.HasValue || d.EndDate >= currentDate))
                .OrderByDescending(d => d.StartDate)
                .FirstOrDefault();
        }

    }



}
