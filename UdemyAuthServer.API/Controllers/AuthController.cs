using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthentaticationService _authentaticationService;
        public AuthController(IAuthentaticationService authentaticationService)
        {
            _authentaticationService = authentaticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authentaticationService.CreateTokenAsync(loginDto);
           
            return ActionResultInstance(result);
        }
        [HttpPost]
        public IActionResult CrateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result =  _authentaticationService.CreteTokenByClient(clientLoginDto);
            return ActionResultInstance(result);

        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authentaticationService.RevokeRefreshToken(refreshTokenDto.Token);
            return ActionResultInstance(result);

        }
        [HttpPost]
        public async Task <IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authentaticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
            return ActionResultInstance(result);
        }
    }
}
