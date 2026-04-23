using BLL;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public authController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return Unauthorized("Invalid Email Or Password");
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized("Invalid Email Or Password");
            }

            //Generate Claims

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            // ✅ ADD THIS — fetch roles and add to claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // e.g. "Admin" or "user"
            }

            var token = GenerateToken(claims);
            return Ok(token);


        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(registerDto);
            }
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!addRoleResult.Succeeded)
            {
                return BadRequest(addRoleResult);
            }
            return Ok("Successfully registered user");



        }



        private TokenDto GenerateToken(List<Claim> claims)
        {
            //key 
            var KeyFromConfig = _jwtSettings.SecretKey;
            var keyInBytes = Convert.FromBase64String(KeyFromConfig);
            var key = new SymmetricSecurityKey(keyInBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDateTime = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);


            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: expiryDateTime

                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenDto = new TokenDto(token, _jwtSettings.DurationInMinutes);

            return tokenDto;
        }
    }
}
