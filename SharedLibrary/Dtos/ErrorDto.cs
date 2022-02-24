using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public List<String> Errors { get;private set; }

        //kullanıcıya gostermek istdigimiz durumlarda true set ediyoruz developerın anlaması gerekn hataları false olarak set ediyoruz

        public bool IsShow { get;private set; }

        public ErrorDto()
        {
            Errors = new List<string>();

        }
        public ErrorDto(string error,bool isShow)
        {
            Errors.Add(error);
            isShow = true;

        }
        public ErrorDto(List<string> error,bool isShow)
        {
            Errors = error;
            IsShow = isShow;

        }

    }
}
