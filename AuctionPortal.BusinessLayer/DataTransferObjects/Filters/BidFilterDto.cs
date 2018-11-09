using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Filters
{
    public class BidFilterDto : FilterDtoBase
    {
        public Guid ProductId { get; set; }
    }
}
