using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projectMVC.DataTypes.Shared;
using projectMVC.enums;
using projectMVC.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace projectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;
        private readonly OnlineShopDbContext _dbContext;

        public HomeController(IConfiguration configuration,
            ILogger<HomeController> logger,
            OnlineShopDbContext dbContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
        }


        [Route("signIn")]
        public IActionResult signIn()
        {
            return View("signIn");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO dto)
        {
            // Replace this with your actual user authentication logic
            var userModel = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username); 

            if (userModel != null && VerifyPassword(dto.Password, userModel.pasWord))
            {
                var userPermissionModel = await _dbContext.Users_permission.FirstOrDefaultAsync(u => u.UserId == userModel.Id);
                if(userPermissionModel.PermissionType == UserType.Guest.ToString())
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }
                var token = await GenerateJwtToken(userModel, userPermissionModel);
                var userType = userPermissionModel.PermissionType;
                return Ok(new { token , userType });
            }
            else
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Method to verify password hash
          
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Implement your password verification logic here (e.g., using hashing algorithm)
            // Compare the provided password with the stored hash
            // Return true if they match, false otherwise
            return password == storedHash;
        }


        private  async Task<string> GenerateJwtToken(UserModel userModel, UserPermissionModel userPermissionModel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userModel.UserName),
            new Claim(ClaimTypes.Role, userPermissionModel.PermissionType),
            new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString())
            
        };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public IActionResult Index()
        {

            return View("MainPage");
        }

        [Route("admin")]
        public IActionResult AdminAccess()
        {
            return View("AdminAccess");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
