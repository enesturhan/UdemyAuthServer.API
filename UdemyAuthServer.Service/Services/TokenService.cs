using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Models;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _tokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions>  tokenOptions)
        {
            _userManager = userManager;
            _tokenOptions = tokenOptions.Value;
        }

        //string bir refresh token uretiyoruz
        private string CreateRefreshToken()
        {

            //bu method guid e göre daha garanti random bir string uretiyor 

            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);

        }
        /// <summary>
        /// tokenin payloadınd olması gereken butun dataları bu method sayesinde eklemiş olduk.
        /// üyelik sistemi gerektiren durumlarda bu method kullanılacak
        /// </summary>
        /// <param name="userApp"></param>
        /// <param name="audiences"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaim(UserApp userApp, List<String> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                //sifre ile olusturma yapıyoruz
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())


        };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;
        }
        //tek bir string ifade liste halinde 
        /// <summary>
        /// üyelik sistemi gerektirmeyen kısımlarda bu method ile claim olusturulacak
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
            return claims;

        }

        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(userApp, _tokenOptions.Audience),
                signingCredentials:signingCredentials
            
                
            );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials


            );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,   
                AccessTokenExpiration = accessTokenExpiration,
            
            };
            return tokenDto;
        }
    }
}
