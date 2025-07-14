using API.Exceptions;
using Application.DTOs;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration config,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    throw new ApiException("Email and Password are required.", 400, "Validation failed", null);

                // Check if tenant exists
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Identifier == dto.TenantIdentifier);

                if (tenant == null)
                {
                    tenant = new Tenant
                    {
                        Name = dto.TenantName,
                        Identifier = dto.TenantIdentifier
                    };

                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();
                }

                var user = new User
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    TenantId = tenant.Id
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errorMsg = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApiException("User registration failed", 400, errorMsg, null);
                }

                return Ok(new { Message = "User registered successfully." });
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException("An unexpected error occurred during registration.", 500, ex.Message, null, ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    throw new ApiException("Email and Password are required.", 400, "Validation failed", null);

                var user = await _userManager.Users
                    .Include(u => u.Tenant)
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (user == null)
                    throw new ApiException("Invalid email or password.", 401, "Unauthorized", null);

                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

                if (!result.Succeeded)
                    throw new ApiException("Invalid email or password.", 401, "Unauthorized", null);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("TenantId", user.TenantId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires = token.ValidTo
                });
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException("An unexpected error occurred during login.", 500, ex.Message, null, ex);
            }

        }
    }

}
