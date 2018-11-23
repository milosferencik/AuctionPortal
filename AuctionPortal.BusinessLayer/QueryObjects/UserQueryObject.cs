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
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.QueryObjects
{
    public class UserQueryObject : QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>>
    {
        public UserQueryObject(IMapper mapper, IQuery<User> query) : base(mapper, query)
        {
        }

        protected override IQuery<User> ApplyWhereClause(IQuery<User> query, UserFilterDto filter)
        {
            return query.Where(new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filter.Username));
        }
    }
}
