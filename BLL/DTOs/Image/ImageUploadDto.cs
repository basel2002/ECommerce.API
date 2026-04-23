using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BLL
{
    public class ImageUploadDto
    {
        //[FromForm]
        public IFormFile? File { get; set; }
    }
}
