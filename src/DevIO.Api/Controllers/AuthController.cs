using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.Controllers
{
    
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly AppSettings _appsettings;
        
        public AuthController(INotificador notificador,
                                 SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager,
                                 IOptions<AppSettings> appsettings
                                 ) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager  = userManager;
            _appsettings = appsettings.Value;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
             if(!ModelState.IsValid) return CustomResponse(ModelState);

             var user = new IdentityUser{

                 UserName = registerUser.Email,
                 Email = registerUser.Email,
                 EmailConfirmed = true
             };

             var result = await _userManager.CreateAsync(user, registerUser.Password);
            
             if(result.Succeeded)
             {
                 await _signInManager.SignInAsync(user, false); //is persistent = false
                 return CustomResponse(GerarJwt());
             }
             foreach(var error in result.Errors)
             {
                 NotificarErro(error.Description);
             }


            return CustomResponse(registerUser);
        }


        


        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
              if(!ModelState.IsValid) return CustomResponse(ModelState);

              var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
               
               if(result.Succeeded)
               {
                   return CustomResponse(GerarJwt());
               }
                if(result.IsLockedOut)
               {
                   NotificarErro("Usuario temporariamente boqueado por tentativas inválidas");
                   return CustomResponse(loginUser.Email);
               }
               NotificarErro("Usuário ou senha incorretos");
               return CustomResponse(loginUser);
        }

        private string GerarJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("_appsettings.Secret");
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appsettings.Emissor,
                Audience = _appsettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appsettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            
            });
            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }
    }
}