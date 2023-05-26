using API.Error;
using API.Helper;
using AutoMapper;
using Core.DTOS.Auth;
using Core.Interfaces;
using Infrastructure.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IOptions<JWT> _jwt;
        private readonly IMapper _mapper;
        private readonly IAddressRep addressRep;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<JWT> jwt, IMapper mapper, IAddressRep addressRep)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this.addressRep = addressRep;
            _jwt = jwt;
        }

        [HttpGet("secret")]
        [Authorize]
        public string getSecret()
        {
            return "secret sting";
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(Login loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, null));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401, null));
            return new UserDto
            {
                Email = user.Email,
                Token = await CreateJwtToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Regitser(RegisterDto registerDto)
        {

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, null));
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await CreateJwtToken(user),
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return new UserDto
            {
                Email = user.Email,
                Token = await CreateJwtToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTo>> GetUserAddress()
        {

            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var x = await addressRep.GetAddress(user.Id);
            return x;

        }
        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<AddressDTo>> UpdateUserAddress(AddressDTo address)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            user.Address = _mapper.Map<AddressDTo, Address>(address);
            var x = await addressRep.CreateAsync(address, user.Id);
            return x;
        }



        #region CreateJwtToken
        private async Task<string> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> roleClaims = new List<Claim>();

            foreach (var role in userRoles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.Value.DurationInDays),
                signingCredentials: signingCredentials);

            var t = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return t;
        }
        #endregion

    }
}
