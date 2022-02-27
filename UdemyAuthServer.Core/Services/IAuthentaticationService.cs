using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.Core.Services
{
    public interface IAuthentaticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        //refresh token silme null icin set etmek icin kullanılabilir.
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

        //5 taneye kadar app setting dosyasında tutulaiblir fazlası ise databasede tutmak mantıklıdır.
        Response<ClientTokenDto> CreteTokenByClient(ClientLoginDto clientLoginDto);



    }
}
