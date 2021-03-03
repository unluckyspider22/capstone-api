using Infrastructure.DTOs;
using Infrastructure.DTOs.Account;
using Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountService _accountService;
        private IConfiguration _config;


        public LoginService(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _config = configuration;
        }

        public async Task<LoginResponse> BrandLogin(UserModel userInfo)
        {
            LoginResponse result = null;
            var account = await _accountService.GetByUsernameAsync(userInfo.UserName);
            if (account != null)
            {
                result = new LoginResponse();
                string message;
                int status;

                if (account.Password == userInfo.Password)
                {
                    var user = new UserInfo
                    {
                        BrandCode = account.Brand.BrandCode,
                        BrandId = account.Brand.BrandId,
                        Token = GenerateJSONWebToken(userInfo)
                    };

                    status = (int)HttpStatusCode.OK;
                    message = AppConstant.ErrMessage.Login_Success;
                    result.Data = user;
                }
                else
                {
                    status = (int)HttpStatusCode.Unauthorized;
                    message = AppConstant.ErrMessage.Login_Fail;
                }
                result.Message = message;
                result.Status = status;
            }
            return result;
        }
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                    new Claim("Role", userInfo.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["AppSettings:Issuer"],
              _config["AppSettings:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
