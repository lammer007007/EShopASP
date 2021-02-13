using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace EShopASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        static List<Product> products = new List<Product>();

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return products;
        }

        [HttpPost]
        public ActionResult<Product> AddEdit([FromForm]ProductForm p, [FromForm]IFormFile img)
        {
            int id = p.Id;
            Product returnValue;
            if(id == -1)
            {
                Product newProduct = new Product();
                newProduct.Id = products.Count > 0 ? products[^1].Id + 1 : 1;
                newProduct.Title = p.Title;
                newProduct.Category = p.Category;
                newProduct.Price = p.Price;
                newProduct.Description = p.Description;
                products.Add(newProduct);
                returnValue = newProduct;
            }
            else
            {
                Product temp = products.Find(i => i.Id == id);
                if(temp == null)
                    return BadRequest();
                temp.Title = p.Title;
                temp.Category = p.Category;
                temp.Price = p.Price;
                temp.Description = p.Description;
                returnValue = temp;
            }

            if(img != null)
            {
                byte[] buffer = new byte[img.Length];
                Stream s = img.OpenReadStream();
                s.Read(buffer, 0, buffer.Length);
                s.Close();
                if(!ImagesController.images.ContainsKey(returnValue.Id))
                {
                    ImagesController.images.Add(returnValue.Id, buffer);
                    ImagesController.formats.Add(returnValue.Id, img.ContentType);
                }
                else
                {
                    ImagesController.images[returnValue.Id] = buffer;
                    ImagesController.formats[returnValue.Id] = img.ContentType;
                }
            }
            else if(p.Clear == "on")
            {
                ImagesController.images.Remove(returnValue.Id);
                ImagesController.formats.Remove(returnValue.Id);
            }

            Response.StatusCode = 302;
            Response.Headers.Add("Location", "/?id=" + returnValue.Id);
            return returnValue;
        }

        [HttpDelete]
        public ActionResult Remove(IDictionary<string, int> i)
        {
            if(!i.ContainsKey("id"))
                return BadRequest();

            products.RemoveAt(products.FindIndex(p => p.Id == i["id"]));
            ImagesController.images.Remove(i["id"]);
            ImagesController.formats.Remove(i["id"]);
            return Ok();
        }
    }
}