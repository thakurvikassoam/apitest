using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using CFAWebApi.Models;
using CFAWebApi.Repository.IRepo;
using System.Linq;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace CFAWebApi.JWT.Services
{
    public interface IIdentityService
    {
        Task<ResponseModel<TokenModel>> LoginAsync(UserData login);
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserContext _context;
        private readonly ServiceConfiguration _appSettings;
        private readonly IUserRepo _employeeService;

        private readonly TokenValidationParameters _tokenValidationParameters;
        public IdentityService(UserContext context,
            IOptions<ServiceConfiguration> settings,
            TokenValidationParameters tokenValidationParameters, IUserRepo userRepo)
        {
            _context = context;
            _appSettings = settings.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _employeeService = userRepo;
        }


        public async Task<ResponseModel<TokenModel>> LoginAsync(UserData login)
        {
            ResponseModel<TokenModel> response = new ResponseModel<TokenModel>();
            try
            {
                //string md5Password = MD5Helpers.GenerateMd5Hash(login.Password);
                // UsersMaster loginUser = _context.UsersMaster.FirstOrDefault(c => c.UserName == login.UserName && c.Password == md5Password);
                List<UserData> empList;
                empList = _context.Set<UserData>().ToList();
                UserData loginUser = empList.Find(u => u.EmailAddress == login.EmailAddress);
                //Find<UserData>();
                if (loginUser.Password == login.Password)
                {
                    AuthenticationResult authenticationResult = await AuthenticateAsync(loginUser);
                    if (authenticationResult != null && authenticationResult.Success)
                    {
                        response.Data = new TokenModel() { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken,Role=authenticationResult.Role };
                    }
                    else
                    {
                        response.Message = "Something went wrong!";
                        response.IsSuccess = false;
                    }



                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Username And Password";
                    return response;
                }



                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private List<RolesMaster> GetUserRole(long UserId)
        //{
        //    try
        //    {
        //        List<RolesMaster> rolesMasters = (from UM in _context.UsersMaster
        //                                          join UR in _context.UserRoles on UM.UserId equals UR.UserId
        //                                          join RM in _context.RolesMaster on UR.RoleId equals RM.RoleId
        //                                          where UM.UserId == UserId
        //                                          select RM).ToList();
        //        return rolesMasters;
        //    }
        //    catch (Exception)
        //    {
        //        return new List<RolesMaster>();
        //    }
        //}
        public async Task<AuthenticationResult> AuthenticateAsync(UserData user)
        {
            // authentication successful so generate jwt token  
            AuthenticationResult authenticationResult = new AuthenticationResult();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);
                List<UserData> empList;
                empList = _context.Set<UserData>().ToList();
                UserData loginUser = empList.Find(u => u.EmailAddress == user.EmailAddress);
                ClaimsIdentity Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Id", loginUser.Id.ToString()),
                    new Claim("Name", loginUser.Name),
                    new Claim("Role",loginUser.Role),  
                    new Claim("EmailAddress",loginUser.EmailAddress==null?"":loginUser.EmailAddress),
                    new Claim("Password",user.Password==null?"":loginUser.Password),                  
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    });


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = Subject,
                    Expires = DateTime.UtcNow.Add(_appSettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                authenticationResult.Token = tokenHandler.WriteToken(token);
                var refreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    Name = loginUser.Name,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6)
                };
                // await _context.RefreshToken.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.Success = true;
                authenticationResult.Role = loginUser.Role;

                return authenticationResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
