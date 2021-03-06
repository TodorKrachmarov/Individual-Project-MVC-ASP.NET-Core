﻿namespace AdvertisementSystem.Test.Services
{
    using Data;
    using Data.Models;
    using FluentAssertions;
    using AdvertisementSystem.Services.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using Xunit;

    public class TagServiceTests
    {
        public TagServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public void AdsByTagShouldReturnTwoAdsInDesendingOrderByTheirPublishDate()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstAd = new Ad { Id = 1, Title = "First", PublishDate = DateTime.UtcNow.AddDays(-3) };
            firstAd.Tags.Add(new AdTag { TagId = 2 });
            var secondAd = new Ad { Id = 2, Title = "Second", PublishDate = DateTime.UtcNow.AddDays(-2), CategoryId = 1 };
            secondAd.Tags.Add(new AdTag { TagId = 1 });
            var thirdAd = new Ad { Id = 3, Title = "Third", PublishDate = DateTime.UtcNow.AddDays(-1), CategoryId = 1 };
            thirdAd.Tags.Add(new AdTag { TagId = 1 });


            db.AddRange(firstAd, secondAd, thirdAd);

            db.SaveChanges();

            var tagService = new TagService(db);

            // Act
            var result = tagService.AdsByTag(1, 1);

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 3
                    && r.ElementAt(1).Id == 2)
                .And
                .HaveCount(2);
        }

        [Fact]
        public void TotalAdsByTagCountShoudReturnTheRightCount()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstAd = new Ad { Id = 1 };
            firstAd.Tags.Add(new AdTag { TagId = 2 });
            var secondAd = new Ad { Id = 2 };
            secondAd.Tags.Add(new AdTag { TagId = 1 });
            var thirdAd = new Ad { Id = 3 };
            thirdAd.Tags.Add(new AdTag { TagId = 1 });

            db.AddRange(firstAd, secondAd, thirdAd);

            db.SaveChanges();

            var tagService = new TagService(db);

            // Act
            var result = tagService.TotalAdsByTagCount(1);

            // Assert
            result
                .Should()
                .Be(1);
        }

        [Fact]
        public void GetNameShoudReturnTheRightName()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstTag = new Tag { Id = 1, Name = "First" };
            var secondTag = new Tag { Id = 2, Name = "Second" };
            var thirdTag = new Tag { Id = 3, Name = "Third" };

            db.AddRange(firstTag, secondTag, thirdTag);

            db.SaveChanges();

            var tagService = new TagService(db);

            // Act
            var result = tagService.GetName(3);

            // Assert
            result
                .Should()
                .Be("Third");
        }

        private AdvertisementDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<AdvertisementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AdvertisementDbContext(dbOptions);
        }
    }
}
