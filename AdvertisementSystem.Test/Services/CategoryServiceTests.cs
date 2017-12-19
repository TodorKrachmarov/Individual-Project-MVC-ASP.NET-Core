namespace AdvertisementSystem.Test.Services
{
    using Data;
    using Data.Models;
    using FluentAssertions;
    using AdvertisementSystem.Services.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using Xunit;

    public class CategoryServiceTests
    {
        public CategoryServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public void AdsByCategoryShouldReturnTwoAdsInDesendingOrderByTheirPublishDate()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstAd = new Ad { Id = 1, Title = "First", PublishDate = DateTime.UtcNow.AddDays(-3), CategoryId = 2 };
            var secondAd = new Ad { Id = 2, Title = "Second", PublishDate = DateTime.UtcNow.AddDays(-2), CategoryId = 1 };
            var thirdAd = new Ad { Id = 3, Title = "Third", PublishDate = DateTime.UtcNow.AddDays(-1), CategoryId = 1 };

            db.AddRange(firstAd, secondAd, thirdAd);

            db.SaveChanges();

            var categoryService = new CategoryService(db);

            // Act
            var result = categoryService.AdsByCategory(1, 1);

            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 3
                    && r.ElementAt(1).Id == 2)
                .And
                .HaveCount(2);
        }

        [Fact]
        public void TotalAdsByCategoryCountShoudReturnTheRightCount()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstAd = new Ad { Id = 1, CategoryId = 1 };
            var secondAd = new Ad { Id = 2, CategoryId = 1 };
            var thirdAd = new Ad { Id = 3, CategoryId = 1 };

            db.AddRange(firstAd, secondAd, thirdAd);

            db.SaveChanges();

            var categoryService = new CategoryService(db);

            // Act
            var result = categoryService.TotalAdsByCategoryCount(1);

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

            var firstCategory = new Category { Id = 1, Name = "First" };
            var secondCategory = new Category { Id = 2, Name = "Second" };
            var thirdCategory = new Category { Id = 3, Name = "Third" };

            db.AddRange(firstCategory, secondCategory, thirdCategory);

            db.SaveChanges();

            var categoryService = new CategoryService(db);

            // Act
            var result = categoryService.GetName(3);

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
