using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class BaseSpecification<T>:ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Critera { get; }

        public List<Expression<Func<T, object>>> Includes {get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool isPagingEnable { get; private set; }

        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T,bool>> Critera)
        {
            this.Critera = Critera;
        }

        protected void AddIncludes(Expression<Func<T, object>> IncludeExpression)
        {
            Includes.Add(IncludeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> OrderBy)
        {
            this.OrderBy = OrderBy;
        }
        protected void AddOrderByDesc(Expression<Func<T, object>> OrderByDesc)
        {
            this.OrderByDesc = OrderByDesc;
        }

        protected void ApplyPaging(int skip,int take)
        {
            this.Skip = skip;
            this.Take = take;
            this.isPagingEnable = true;
        }

    }
}
