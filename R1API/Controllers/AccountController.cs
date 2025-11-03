using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using R1API.DTOS;
using R1API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace R1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController (UserManager<ApplicationUser> userManager,IConfiguration config)
        : ControllerBase
    {
        [HttpPost("register")]//api/account/register post
        public async Task<IActionResult> Register(RegisterUserdto userFromRe)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser()
                {
                    UserName = userFromRe.UserName
                };
                IdentityResult result=
                    await  userManager.CreateAsync(appUser, userFromRe.Password);
                if(result.Succeeded)
                {
                    return Ok("Created");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]//api/account/login post
        public async Task<IActionResult> Login(LoginUserdto userFromReq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user=await userManager.FindByNameAsync(userFromReq.UserName);
                if (user != null)
                {
                   bool found =await  userManager.CheckPasswordAsync(user, userFromReq.Password);
                    if (found)
                    {
                        List<Claim> myclaims = new List<Claim>();
                        myclaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        myclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        myclaims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));

                        var userRoles=await userManager.GetRolesAsync(user);
                        if (userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                myclaims.Add(new Claim(ClaimTypes.Role,role));

                            }
                        }
                        string key = config["JWT:Key"];
                        var keyInByte = Encoding.UTF8.GetBytes(key);
                        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyInByte);

                        SigningCredentials signingCredentials = 
                            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        //create Token
                        JwtSecurityToken mytoken =
                            new JwtSecurityToken(
                                issuer: config["JWT:Iss"],
                                audience: config["JWT:Aud"],
                                claims: myclaims,
                                expires:DateTime.Now.AddHours(1),
                                signingCredentials: signingCredentials);

                        return Ok(new
                        {
                            expiration = DateTime.Now.AddHours(1),
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken)

                        }) ;

                    }
                }
                return BadRequest("Invalid Account");
            }
            return BadRequest(ModelState);
        }
    }
}
