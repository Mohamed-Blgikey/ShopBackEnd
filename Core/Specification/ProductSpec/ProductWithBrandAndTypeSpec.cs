using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification.ProductSpec
{
    public class ProductWithBrandAndTypeSpec : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpec(ProductSpecParams @params):
            base( x=>
            (string.IsNullOrEmpty(@params.Search) || x.Name.ToLower().Contains(@params.Search)) &&
            (!@params.TypeId.HasValue || x.ProductTypeId == @params.TypeId) &&
            (!@params.BrandId.HasValue || x.ProductBrandId == @params.BrandId) 
            )
        {
            AddIncludes(a => a.ProductBrand);
            AddIncludes(a => a.ProductType);
            ApplyPaging(@params.PageSize * (@params.PageIndex - 1), @params.PageSize);
            if (!string.IsNullOrEmpty(@params.Sort))
            {
                switch (@params.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(a => a.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(a => a.Price);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                AddOrderByDesc(a => a.Name);

            }
        }
        public ProductWithBrandAndTypeSpec(int id) : base(x => x.Id == id)
        {
            AddIncludes(a => a.ProductBrand);
            AddIncludes(a => a.ProductType);
        }
    }
}
