namespace AdvertisementSystem.Services.Implementations
{
    using AdvertisementSystem.Services.Models.Users;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Ad;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.DataConstants;

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

        public IEnumerable<ListAdsServiceModel> AdsByUser(string id, int page)
            => this.db
                    .Ads
                    .OrderByDescending(a => a.PublishDate)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Where(a => a.AuthorId == id)
                    .ProjectTo<ListAdsServiceModel>()
                    .ToList();

        public bool Exist(string id)
            => this.db.Users.Any(c => c.Id == id);

        public string GetName(string id)
            => this.db.Users.FirstOrDefault(c => c.Id == id).Name;

        public int TotalAdsByTagCount(string id)
        {
            var count = this.db
                            .Ads
                            .Where(a => a.AuthorId == id)
                            .Count();

            return (int)Math.Ceiling(count / (double)PageSize);
        }

        public UserProfileServiceModel GetProfile(string id)
            => this.db
                    .Users
                    .Where(u => u.Id == id)
                    .ProjectTo<UserProfileServiceModel>()
                    .FirstOrDefault();
    }
}
