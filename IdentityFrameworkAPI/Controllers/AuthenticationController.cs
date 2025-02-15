using Azure;
using IdentityFrameworkAPI.Models.Authentication.Login;
using IdentityFrameworkAPI.Models.Authentication.SignUp;
using IdentityFrameworkAPI.Models.Responce;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityFrameworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterUserModel registerUser, string RoleName)
        {
            // Check User Exist In DataBase Or Not For Perticular MailId
            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ResponceModel
                    {
                        status = "Error",
                        msg = "User already exists in the database. Please sign in or reset your password."
                    });
            }
            else
            {
                // Is User Not Exists Then Create New Entity For User In DataBase
                IdentityUser user = new()
                {
                    UserName = registerUser.Username,
                    Email = registerUser.Email,
                    PhoneNumber = registerUser.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),

                };

                // Check Given Role Is Defined In System Or Not

                if (await _roleManager.RoleExistsAsync(RoleName.Trim().ToUpper()))
                {
                    // Crate User Entity In DataBase
                    var IsUserCreated = await _userManager.CreateAsync(user, registerUser.Password);
                    if (!IsUserCreated.Succeeded)
                    {

                        // If Due To Some Reason User Not Created Then Return Server Error
                        return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponceModel
                        {
                            status = "Error",
                            msg = "Unable To Create User Please Try Again SomeTime Later  !"
                        });



                    }
                    else
                    {
                        var isRoleAdded = await _userManager.AddToRoleAsync(user, RoleName);
                        return StatusCode(StatusCodes.Status201Created,
                        new ResponceModel
                        {
                            status = "Success",
                            msg = "User Created SuccesFully !"
                        });
                    }
                }
                else
                {


                    return StatusCode(StatusCodes.Status404NotFound,
                    new ResponceModel
                    {
                        status = "Error",
                        msg = "This User Role Not Exists !"
                    });

                }



            }

        }


    }
}
