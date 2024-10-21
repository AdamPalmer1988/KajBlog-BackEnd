using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KajBlog_BackEnd.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ApiBaseController : ControllerBase
{
    //AUTH ZERO
    //This gets the user id from the token
    //We inherit this class so all controllers can easily get the user id
    protected string GetCurrentUserID()
    {
        if (User.Identity is ClaimsIdentity identity)
        {
            // Extract the user ID from the claims (assuming it's stored in the "sub" claim)
            var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
        }

        return "ABC123";
    }
}
