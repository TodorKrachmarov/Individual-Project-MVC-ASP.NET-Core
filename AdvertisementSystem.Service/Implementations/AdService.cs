namespace AdvertisementSystem.Services.Implementations
{
    using Contracts;
    using Data;
    using Data.Models;
    using System;
    using System.Linq;
    using AdvertisementSystem.Services.Models.Ad;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class AdService : IAdService
    {
        private readonly AdvertisementDbContext db;

        public AdService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public void Create(string title, string description, string imageUrl, int CategoryId, string keyWords, string authorId)
        {
            var ad = new Ad
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                AuthorId = authorId,
                CategoryId = CategoryId,
                PublishDate = DateTime.UtcNow
            };

            var tags = keyWords.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var tag in tags)
            {
                if (!this.db.Tags.Any(tg => tg.Name == tag))
                {
                    this.db.Tags.Add(new Tag { Name = tag });
                    this.db.SaveChanges();
                }

                var t = this.db.Tags.FirstOrDefault(tg => tg.Name == tag);

                ad.Tags.Add(new AdTag { TagId = t.Id, Tag = t });
            }

            this.db.Ads.Add(ad);
            this.db.SaveChanges();
        }

        public bool Exists(int adId, string authorId)
            => this.db.Ads.Any(a => a.Id == adId && a.AuthorId == authorId);

        public EditAdServiceModel FindToEdit(int id)
            => this.db.Ads
                        .Where(a => a.Id == id)
                        .ProjectTo<EditAdServiceModel>()
                        .FirstOrDefault();

        public bool Edit(int id, string title, string description, string imageUrl, int CategoryId, string keyWords)
        {
            var ad = this.db.Ads.Where(a => a.Id == id).Include(a => a.Tags).ThenInclude(a => a.Tag).FirstOrDefault();

            if (ad == null)
            {
                return false;
            }

            ad.Title = title;
            ad.Description = description;
            ad.ImageUrl = imageUrl;
            ad.CategoryId = CategoryId;
            ad.Tags.Clear();

            this.db.SaveChanges();

            var tags = keyWords.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var tag in tags)
            {
                if (!this.db.Tags.Any(tg => tg.Name == tag))
                {
                    this.db.Tags.Add(new Tag { Name = tag });
                    this.db.SaveChanges();
                }

                var t = this.db.Tags.FirstOrDefault(tg => tg.Name == tag);

                ad.Tags.Add(new AdTag { TagId = t.Id, Tag = t });
            }

            this.db.SaveChanges();

            return true;
        }

        public string Delete(int id)
        {
            var ad = this.db.Ads.FirstOrDefault(a => a.Id == id);

            if (ad == null)
            {
                return null;
            }

            var name = ad.Title;

            this.db.Remove(ad);
            this.db.SaveChanges();

            return name;
        }

        public bool ReadyToDelete(int id)
            => this.db.Ads.Any(a => a.Id == id);

        public AdDetailsServiceModel Details(int id)
            => this.db
                    .Ads
                    .Where(a => a.Id == id)
                    .ProjectTo<AdDetailsServiceModel>()
                    .FirstOrDefault();
    }
}
