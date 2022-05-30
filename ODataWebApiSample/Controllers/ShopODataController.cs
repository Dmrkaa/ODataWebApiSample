using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataWebApiSample.Data;
using ODataWebApiSample.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ODataWebApiSample.Controllers
{

    public class ShopODataController : ODataController
    {
        private readonly AppDataContext _shopDataContext;
        public ShopODataController(AppDataContext dataContext)
        {
            _shopDataContext = dataContext ?? throw new ArgumentNullException(nameof(AppDataContext));
        }

        [EnableQuery()]
        [HttpGet("odata/Brands")]
        public IActionResult Get()
        {
            return Ok(_shopDataContext.Brands);
        }

        [HttpGet("odata/Brands/{key}")]
        public async Task<IActionResult> GetBrands(int key)
        {
            var people = _shopDataContext.Brands.Where(p => p.Id == key);

            if (!await people.AnyAsync())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(people));
        }

        [HttpGet("odata/Products")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _shopDataContext.Products.ToListAsync());
        }

        [HttpPost("Brand")]
        public async Task<IActionResult> CreateBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _shopDataContext.Brands.Add(brand);
            await _shopDataContext.SaveChangesAsync();

            return Created(brand);
        }

        [HttpPatch("Brand/{key}")] // using patch instead of put for update just a part of entity. use put for full update
        public async Task<IActionResult> PartiallyUpdatePerson(int key, [FromBody] Delta<Brand> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPerson = _shopDataContext.Brands
                .FirstOrDefault(p => p.Id == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            patch.Patch(currentPerson);
            await _shopDataContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Brand/{key}")]
        public async Task<IActionResult> DeletePerson(int key)
        {
            var currentPerson = _shopDataContext.Brands
                .FirstOrDefault(p => p.Id == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            _shopDataContext.Brands.Remove(currentPerson);
            await _shopDataContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
