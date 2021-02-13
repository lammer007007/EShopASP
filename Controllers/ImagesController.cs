using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EShopASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        public static IDictionary<int, byte[]> images = new Dictionary<int, byte[]>();

        public static IDictionary<int, string> formats = new Dictionary<int, string>();

        private readonly ILogger<ImagesController> _logger;

        public ImagesController(ILogger<ImagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task GetImg(int? id)
        {
            if(id != null && images.ContainsKey((int)id))
            {
                Response.ContentType = formats[(int)id];
                Response.StatusCode = 200;
                await Response.Body.WriteAsync(images[(int)id]);
                await Response.CompleteAsync();
                return;
            }
            BinaryReader reader = new BinaryReader(new FileStream("wwwroot/no-img.png", FileMode.Open));
            byte[] buffer = new byte[reader.BaseStream.Length];
            reader.Read(buffer, 0, buffer.Length);
            reader.Close();
            Response.ContentType = "image/png";
            Response.ContentLength = buffer.Length;
            Response.StatusCode = 200;
            await Response.Body.WriteAsync(buffer);
            await Response.CompleteAsync();
        }
    }
}