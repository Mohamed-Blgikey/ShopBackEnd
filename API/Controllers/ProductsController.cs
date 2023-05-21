using API.DTOS;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Core.Specification.ProductSpec;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<ProductBrand> productBrandRepo;
        private readonly IGenericRepository<ProductType> productTypeRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> ProductRepo, IGenericRepository<ProductBrand> ProductBrandRepo, IGenericRepository<ProductType> ProductTypeRepo,IMapper mapper)
        {
            productRepo = ProductRepo;
            productBrandRepo = ProductBrandRepo;
            productTypeRepo = ProductTypeRepo;
            this.mapper = mapper;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<Pagenation<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams @params)
        {
            ProductWithBrandAndTypeSpec spec = new ProductWithBrandAndTypeSpec(@params);
            IReadOnlyList<Product> data = await productRepo.ListWithSpecAsync(spec);
            var totalCount = await productRepo.Count();
            return Ok(new Pagenation<ProductToReturnDto>(@params.PageIndex,@params.PageSize,data.Count,totalCount, mapper.Map<IReadOnlyList<ProductToReturnDto>>(data)));
        }

        [HttpGet("GetProductBrands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductProductBrands()
        {
            return Ok(await productBrandRepo.GetAllAsync());
        }
        [HttpGet("GetProductTypes")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductProductProductTypes()
        {
            return Ok(await productTypeRepo.GetAllAsync());
        }
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            ProductWithBrandAndTypeSpec spec = new ProductWithBrandAndTypeSpec(id);
            Product data = await productRepo.GetWithSpecAsync(spec);
            return Ok(mapper.Map<ProductToReturnDto>(data));
        }
    }
}
