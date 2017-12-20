namespace AdvertisementSystem.Test.Web
{
    using AdvertisementSystem.Data.Models;
    using AdvertisementSystem.Services.Contracts;
    using AdvertisementSystem.Services.Models.Ad;
    using AdvertisementSystem.Services.Models.Comment;
    using AdvertisementSystem.Web.Controllers;
    using AdvertisementSystem.Web.Models.Ads;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Xunit;

    public class AdsControllerTests
    {
        [Fact]
        public void DetailsShouldBeForEveryGuestOrUser()
        {
            // Arrange
            var method = typeof(AdsController)
                .GetMethod(nameof(AdsController.Details));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AllowAnonymousAttribute)));
        }

        [Fact]
        public void ProfileShouldReturnViewWithCorrectModelWithValidUsername()
        {
            // Arrange
            var adId = 1;
            var pageNum = 1;
            var userId = "userId";
            var title = "title";

            var adService = new Mock<IAdService>();
            adService
                .Setup(a => a.Details(It.Is<int>(id => id == adId)))
                .Returns(new AdDetailsServiceModel { Title = title });

            var categoryService = new Mock<ICategoryService>();

            var commentService = new Mock<ICommentService>();
            commentService
                .Setup(c => c.AdComments(It.Is<int>(id => id == adId), It.Is<int>(page => page == pageNum)))
                .Returns(new List<ListCommentServiceModel>());

            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var userService = new Mock<IUserService>();
            userService
                .Setup(u => u.IsDeleted(It.Is<string>(id => id == userId)))
                .Returns(false);

            var controller = new AdsController(adService.Object, categoryService.Object, userManager.Object, userService.Object, commentService.Object);

            // Act
            var result = controller.Details(adId, pageNum);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<AdDetailsViewModel>().Title == title && m.As<AdDetailsViewModel>().Comments.Comments.Count() == 0);
        }

        private Mock<UserManager<User>> GetUserManagerMock()
            => new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    }
}
