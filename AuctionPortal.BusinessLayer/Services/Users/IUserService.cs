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
        Task<UserDto> GetAsync(string name);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task<Guid> RegisterUser(UserCreateDto entityDto);

        /// <summary>
        /// authorize user
        /// </summary>
        /// <param name="username"> username</param>
        /// <param name="password"> user password</param>
        /// <returns> tuple of bool is successful and is admin </returns>
        Task<(bool success, string isAdmin)> AuthorizeUserAsync(string username, string password);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void DeleteUser(Guid entityId);
    }
}
