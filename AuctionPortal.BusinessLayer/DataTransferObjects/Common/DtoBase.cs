using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Common
{
    public abstract class DtoBase
    {
        public Guid Id { get; set; }
    }
}
