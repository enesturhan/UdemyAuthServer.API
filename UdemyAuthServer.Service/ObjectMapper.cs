using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Service
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(()=>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();

        });

        //object mapper mapperını cagırana kadar kod yüklenmeyecek 
        //gec yüklenmek gerekn nesnelerde lazy kullanılır

        public static IMapper Mapper => lazy.Value;



    }
}
