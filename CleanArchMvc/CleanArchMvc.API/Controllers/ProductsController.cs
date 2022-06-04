using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtcs = await _productsService.GetProducts();

            if (produtcs == null)
                return NotFound("Produtcs not found");

            return Ok(produtcs);
        }


        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest("Invalid Data");

            await _productsService.Add(productDTO);

            return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id }, productDTO);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDTO>> Put(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
                return BadRequest("Invalid Data");

            if (productDTO == null)
                return BadRequest("Invalid Data");

            await _productsService.Update(productDTO);

            return Ok(productDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
                return NotFound("Category not found");

            await _productsService.Remove(id);

            return Ok(product);
        }

    }
}
