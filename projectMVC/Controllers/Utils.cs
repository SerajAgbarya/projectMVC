using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace projectMVC.Controllers
{
    public class Utils
    {

        public static async Task<Dictionary<string, List<string>>> CategoriesWithAttributesDict(OnlineShopDbContext dbContext)
        {
            var categoriesWithAttributes = await dbContext.Product_category_attributes.ToListAsync();
            // Create a dictionary to store category attributes
            Dictionary<string, List<string>> categoryAttributesDict = new Dictionary<string, List<string>>();
            foreach (var entry in categoriesWithAttributes)
            {
                string categoryName = entry.CategoryName;
                string attributeName = entry.AttributeName;

                if (categoryAttributesDict.ContainsKey(categoryName))
                {
                    categoryAttributesDict[categoryName].Add(attributeName);
                }
                else
                {
                    categoryAttributesDict[categoryName] = new List<string> { attributeName };
                }
            }

            return categoryAttributesDict;
        }

        public static int? GetUserId(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("UserID", out var userIdHeader) && int.TryParse(userIdHeader, out var userId))
            {
                return userId;
            }
            return null;
        }


        
    }

}

