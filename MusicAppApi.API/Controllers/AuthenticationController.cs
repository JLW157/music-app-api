using Google;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicAppApi.Core.Constants;
using MusicAppApi.Core.Filters;
using MusicAppApi.Core.Services;
using MusicAppApi.Models.Configurations;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Auth;
using MusicAppApi.Models.Responses;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MusicAppApi.Core;
using MusicAppApi.Core.Interfaces;
using MusicAppApi.Models.GeneralModels;
using Newtonsoft.Json.Linq;

namespace MusicAppApi.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IConfiguration _configuration;
        private readonly JWTConfiguration jWTConfiguration;
        private readonly IJWTGenerator jwtGenerator;
        private readonly IEmailService _emailService;

        public AuthenticationController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            IOptions<JWTConfiguration> options,
            IEmailService emailService,
            IJWTGenerator jwtGenerator,
            SignInManager<User> signInManger)
        {
            _userManager = userManager;
            this._roleManager = roleManager;
            _configuration = configuration;
            this.jWTConfiguration = options.Value;
            _emailService = emailService;
            this.jwtGenerator = jwtGenerator;
            _signInManager = signInManger;
        }

        [HttpPost]
        [Route("google")]
        public async Task<ActionResult> Google(GoogleAuthenticateRequest google)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

                settings.Audience = new List<string> { _configuration.GetValue<string>("Google:ClientId")! };
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(google.IdToken, settings);

                // check if user exists by email in db

                var user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    // create user with such email and type google

                    user = new User()
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = true,
                        AuthType = AuthType.Google,
                    };

                    var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());

                    if (!result.Succeeded)
                        return BadRequest("O-ops, something went wrong");

                }

                if (user.AuthType == AuthType.Normal)
                    return Unauthorized("This user already registered!");

                return Ok(new AuthenticatedUserResposne
                {
                    Token = jwtGenerator.GenerateToken(payload.Email),
                    Expiration = DateTime.Now.AddMinutes(jWTConfiguration.AccessTokenExpirationMinutes)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate(
             LoginModel loginModel)
        {
            var userFromContextUser = HttpContext.User;

            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                return Unauthorized("User not exists, register yourself!");
            }

            if (user.AuthType == AuthType.Google)
            {
                return Unauthorized("Use google authentication instead");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return BadRequest("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(AuthConstants.ClaimNames.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            // adding all roles which was found
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigngingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecret));

            var token = new JwtSecurityToken(
                issuer: jWTConfiguration.Issuer,
                audience: jWTConfiguration.Audience,
                expires: DateTime.Now.AddMinutes(jWTConfiguration.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigngingKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new AuthenticatedUserResposne
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    string htmlContent = System.IO.File.ReadAllText("./wwwroot/Pages/SuccessEmailConfirmation.html");
                    return Content(htmlContent, "text/html");
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "This user not exists!");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // could be changed, but good for now
            if (await _userManager.Users.AnyAsync(x => x.Email == model.Email))
                return BadRequest("User is already registered with such email");

            if (await _userManager.Users.AnyAsync(x => x.UserName == model.Username))
                return BadRequest("User is already registered with such username");

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AuthType = AuthType.Normal,
                EmailConfirmed = false,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            // here
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(AuthConstants.UserRoles.User))
            {
                await _roleManager.CreateAsync(new Role { Name = AuthConstants.UserRoles.User });
            }

            await _userManager.AddToRoleAsync(user, AuthConstants.UserRoles.User);

            await SentVerificationLinkToEmail(user);

            return Ok("User created successfully! Confirm you email, check your email and confirm it");
        }



        [HttpPost("VerifyEmail")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id);
            if (id == null)
                return BadRequest("Token expired or something went wrong");

            var user = await _userManager.FindByIdAsync(id.Value);
            if (user == null)
                return BadRequest("User not found");

            await SentVerificationLinkToEmail(user);

            return Ok("Verification link has sent.");
        }

        [HttpGet("TestEmail")]
        public async Task<IActionResult> TestEmail()
        {
            await _emailService.SendEmailAsync(new Message(new string[] { "mishkfreddy123@gmail.com", "mishkafreddy1234@gmail.com" }, "Test", "MSkdmklaskkdmlsad"));

            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel registerModel)
        //{
        //    return Ok();
        //}

        private async Task SentVerificationLinkToEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Authentication", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Confirm email",
                $"Here is your confirmation link click on this link and confirm your email - {confirmationLink}");

            await _emailService.SendEmailAsync(message);
        }
    }

    public class GoogleAuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
