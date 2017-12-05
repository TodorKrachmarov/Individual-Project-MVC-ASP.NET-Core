namespace AdvertisementSystem.Services.Implementations
{
    using Contracts;
    using Data;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly AdvertisementDbContext db;

        public UserService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public bool IsDeleted(string id)
            => this.db
                    .Users
                    .Where(u => u.Id == id)
                    .Select(u => u.IsDeleted)
                    .FirstOrDefault();
    }
}
