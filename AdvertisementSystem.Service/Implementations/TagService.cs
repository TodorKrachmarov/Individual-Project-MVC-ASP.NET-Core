namespace AdvertisementSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models.Ad;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.DataConstants;

    public class TagService : ITagService
    {
        private readonly AdvertisementDbContext db;

        public TagService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ListAdsServiceModel> AdsByTag(int id, int page)
            => this.db
                    .Ads
                    .OrderByDescending(a => a.PublishDate)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Where(a => a.Tags.Any(t => t.TagId == id))
                    .ProjectTo<ListAdsServiceModel>()
                    .ToList();

        public bool Exist(int id)
            => this.db.Tags.Any(c => c.Id == id);

        public string GetName(int id)
            => this.db.Tags.FirstOrDefault(c => c.Id == id).Name;

        public int TotalAdsByTagCount(int id)
        {
            var count = this.db
                            .Ads
                            .Where(a => a.Tags.Any(t => t.TagId == id))
                            .Count();

            return (int)Math.Ceiling(count / (double)PageSize);
        }
    }
}
