using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using System.Web.Mvc;
using X.PagedList;

namespace AuctionPortal.PresentationLayer.Models
{
    public class AuctioneerListViewModel
    {
        public string[] AuctioneerSortCriteria => new[] { nameof(AuctioneerDto.Email) };

        public IPagedList<AuctioneerDto> Auctioneers { get; set; }

        public AuctioneerFilterDto Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(AuctioneerSortCriteria);
    }
}