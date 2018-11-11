using System;
using System.Threading.Tasks;
using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Services.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AutoMapper;

namespace AuctionPortal.BusinessLayer.Services.Users
{
    public class UserService : ServiceBase, IUserService
    {
        private IRepository<User> repository;

        protected UserService(IMapper mapper, IRepository<User> repository) : base(mapper)
        {
            this.repository = repository;
        }

        public Guid Create(UserCreateDto entityDto)
        {
            var entity = Mapper.Map<User>(entityDto);
            repository.Create(entity);
            return entity.Id;
        }

        public void DeleteProduct(Guid entityId)
        {
            repository.Delete(entityId);
        }

        public async Task<UserCreateDto> GetAsync(Guid entityId, bool withIncludes = true)
        {
            var entity = await repository.GetAsync(entityId);
            return entity != null ? Mapper.Map<UserCreateDto>(entity) : null;
        }

        public async Task Update(UserCreateDto entityDto)
        {
            var entity = await repository.GetAsync(entityDto.Id);
            Mapper.Map(entityDto, entity);
            repository.Update(entity);
        }

    }
}
