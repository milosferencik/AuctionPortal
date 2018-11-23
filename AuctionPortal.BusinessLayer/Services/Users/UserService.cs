using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.BusinessLayer.Services.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.Query;
using AutoMapper;

namespace AuctionPortal.BusinessLayer.Services.Users
{
    public class UserService : ServiceBase, IUserService
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        private IRepository<Auctioneer> repository;
        private readonly QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject;


        protected UserService(IMapper mapper, IRepository<Auctioneer> repository) : base(mapper)
        {
            this.repository = repository;
        }

        public async Task<Guid> RegisterUser(UserCreateDto entityDto)
        {
            var auctioneer = Mapper.Map<Auctioneer>(entityDto);

            if(await GetIfUserExistsAsync(auctioneer.Username))
            {
                throw new ArgumentException("User already exists");
            }

            var password = CreateHash(entityDto.Password);
            auctioneer.PasswordHash = password.Item1;
            auctioneer.PasswordSalt = password.Item2;

            repository.Create(auctioneer);
            return auctioneer.Id;
        }

        public async Task<(bool success, string isAdmin)> AuthorizeUserAsync(string username, string password)
        {
            var userResult = await userQueryObject.ExecuteQuery(new UserFilterDto { Username = username });
            var user = userResult.Items.SingleOrDefault();

            var succ = user != null && VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password);
            var isAdmin = user?.IsAdmin?? "";
            return (succ, isAdmin);

        }

        public void DeleteUser(Guid entityId)
        {
            repository.Delete(entityId);
        }

        public async Task<UserDto> GetAsync(string name)
        {
            var queryResult = await userQueryObject.ExecuteQuery(new UserFilterDto() { Username = name });
            return queryResult.Items.SingleOrDefault();
        }

        private async Task<bool> GetIfUserExistsAsync(string username)
        {
            var queryResult = await userQueryObject.ExecuteQuery(new UserFilterDto { Username = username });
            return (queryResult.Items.Count() > 0);
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }

        private Tuple<string, string> CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return Tuple.Create(Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

    }
}
