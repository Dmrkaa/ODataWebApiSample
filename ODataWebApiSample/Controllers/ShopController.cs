using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODataWebApiSample.Data;
using ODataWebApiSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataWebApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly AppDataContext _context;

        public ShopController(ILogger<ShopController> logger, AppDataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        [HttpGet]
        [EnableQuery]
        [Route("Brands")]
        public IEnumerable<Brand> GetBrands()
        {
            var result = _context.Brands;
            return result;
        }

        [HttpGet]
        [EnableQuery]
        [Route("Products")]
        public IEnumerable<Brand> GetProducts()
        {
            var result = _context.Brands;
            return result;
        }
    }
}
