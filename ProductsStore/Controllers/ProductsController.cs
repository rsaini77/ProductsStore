﻿using Microsoft.AspNetCore.Mvc;
using ProductsStore.Models;
using ProductsStore.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }
    }
}
