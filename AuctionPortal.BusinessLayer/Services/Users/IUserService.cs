using AuctionPortal.BusinessLayer.DataTransferObjects;
using System;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Gets DTO representing the entity according to ID
        /// </summary>
        /// <param name="entityId">entity ID</param>
        /// <param name="withIncludes">include all entity complex types</param>
        /// <returns>The DTO representing the entity</returns>
        Task<UserCreateDto> GetAsync(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid Create(UserCreateDto entityDto);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task Update(UserCreateDto entityDto);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void DeleteProduct(Guid entityId);
    }
}
