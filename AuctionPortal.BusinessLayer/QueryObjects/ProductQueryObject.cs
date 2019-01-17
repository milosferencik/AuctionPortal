using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure.Query;
using AuctionPortal.Infrastructure.Query.Predicates;
using AuctionPortal.Infrastructure.Query.Predicates.Operators;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuctionPortal.BusinessLayer.QueryObjects
{
    public class ProductQueryObject : QueryObjectBase<ProductDto, Product, ProductFilterDto, IQuery<Product>>
    {
        public ProductQueryObject(IMapper mapper, IQuery<Product> query) : base(mapper, query) { }

        protected override IQuery<Product> ApplyWhereClause(IQuery<Product> query, ProductFilterDto filter)
        {
            var definedPredicates = new List<IPredicate>();
            AddIfDefined(FilterCategories(filter), definedPredicates);
            AddIfDefined(FilterProductName(filter), definedPredicates);
            AddIfDefined(FilterSellerId(filter), definedPredicates);
            AddIfDefined(FilterBuyerId(filter), definedPredicates);
            AddIfDefined(FilterIsSold(filter), definedPredicates);
            if (definedPredicates.Count == 0)
            {
                return query;
            }
            if (definedPredicates.Count == 1)
            {
                return query.Where(definedPredicates.First());
            }
            var wherePredicate = new CompositePredicate(definedPredicates);
            return query.Where(wherePredicate);
        }

        private static void AddIfDefined(IPredicate categoryPredicate, ICollection<IPredicate> definedPredicates)
        {
            if (categoryPredicate != null)
            {
                definedPredicates.Add(categoryPredicate);
            }
        }

        private static CompositePredicate FilterCategories(ProductFilterDto filter)
        {
            if (filter.CategoryIds == null || !filter.CategoryIds.Any())
            {
                return null;
            }
            var categoryIdPredicates = new List<IPredicate>(filter.CategoryIds
                .Select(categoryId => new SimplePredicate(
                    nameof(Product.CategoryId),
                    ValueComparingOperator.Equal,
                    categoryId)));
            return new CompositePredicate(categoryIdPredicates, LogicalOperator.OR);
        }

        private static SimplePredicate FilterProductName(ProductFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.SearchedName))
            {
                return null;
            }
            return new SimplePredicate(nameof(Product.Name), ValueComparingOperator.StringContains,
                filter.SearchedName);
        }

        private static SimplePredicate FilterSellerId(ProductFilterDto filter)
        {
            if (filter.SellerId.Equals(Guid.Empty))
            {
                return null;
            }
            return new SimplePredicate(nameof(Product.SellerId), ValueComparingOperator.Equal, filter.SellerId);
        }

        private static SimplePredicate FilterBuyerId(ProductFilterDto filter)
        {
            if (filter.BuyerID.Equals(Guid.Empty))
            {
                return null;
            }
            return new SimplePredicate(nameof(Product.BuyerId), ValueComparingOperator.Equal, filter.BuyerID);
        }

        private static SimplePredicate FilterIsSold(ProductFilterDto filter)
        {
            if (filter.IsSold.Equals(null))
            {
                return null;
            }
            return new SimplePredicate(nameof(Product.IsSold), ValueComparingOperator.Equal, filter.IsSold);
        }
    }
}
