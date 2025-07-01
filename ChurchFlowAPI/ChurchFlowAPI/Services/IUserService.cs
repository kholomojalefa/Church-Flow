using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using ChurchFlowAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChurchFlowAPI.Services
{
    //interface

    public interface IUserService
    {
        Task<AuthResult> RegisterAsync(RegisterDto dto);
        Task<AuthResult> LoginAsync(LoginDto dto);
        Task<UserDto> GetByIdAsync(string userId);
        Task<string> PromoteToAdminAsync(string userId);
        Task<List<UserDto>> GetAllUsersAsync();
    }
}

//logic
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<UserDto> GetByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = await _userManager.GetRolesAsync(user)
        };
    }


    public async Task<AuthResult> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // Optionally assign role here
        await _userManager.AddToRoleAsync(user, "USER");

        var token = await GenerateJwtToken(user);

        return new AuthResult
        {
            Succeeded = true,
            Token = token,
            Message = "Registration successful"
        };
    }

    public async Task<string> PromoteToAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return "User not found";

        var isInRole = await _userManager.IsInRoleAsync(user, "ADMIN");
        if (isInRole) return "User is already an admin";

        await _userManager.AddToRoleAsync(user, "ADMIN");
        return "User promoted to admin";
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();

        var userList = new List<UserDto>();

        foreach (var user in users)
        {
            userList.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        return userList;
    }


    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "Invalid email or password."
            };
        }

        var token = await GenerateJwtToken(user);

        return new AuthResult
        {
            Succeeded = true,
            Token = token,
            Message = "Login successful"
        };
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var roles = await _userManager.GetRolesAsync(user);
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("SecretKey");

        var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            expires: DateTime.UtcNow.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}


