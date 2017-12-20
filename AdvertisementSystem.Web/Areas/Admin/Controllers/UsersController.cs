namespace AdvertisementSystem.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.User;
    using Services.Contracts;
    using System.Threading.Tasks;

    using static WebConstants;

    public class UsersController : BaseController
    {
        private readonly IAdminService admin;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(
            IAdminService admin,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.admin = admin;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> All(string searchTerm, int page = 1)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var totalPages = this.admin.AllUsersCount();
            page = this.VerifyPageValue(page, totalPages);

            var model = new AdminUsersViewModel
            {
                Users = await this.admin.GetUsers(page, searchTerm),
                CurrentPage = page,
                TotalPages = totalPages
            };

            return this.View(model);
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (this.admin.IsDeleted(id))
            {
                this.AddErrorMessage("The user you are trying to reset the password is deactivated!");
                return this.RedirectToAllUsers();
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.AddErrorMessage("The user you are trying to reset the password does not exist!");
                return this.RedirectToAllUsers();
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, ChangeUserPasswordViewModel model)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (this.admin.IsDeleted(id))
            {
                this.AddErrorMessage("The user you are trying to reset the password is deactivated!");
                return this.RedirectToAllUsers();
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.AddErrorMessage("The user you are trying to reset the password does not exist!");
                return this.RedirectToAllUsers();
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                this.AddErrorMessage(result.ToString());
                return View(model);
            }

            this.AddSuccessMessage($"You changed the password of {user.Name}!");
            return this.RedirectToAllUsers();
        }

        public async Task<IActionResult> MakeAdmin(string id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            if (this.admin.IsDeleted(id))
            {
                this.AddErrorMessage($"The user you are trying to make an {AdministratorRole} is deactivated!");
                return this.RedirectToAllUsers();
            }

            var roleName = AdministratorRole;
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    this.AddErrorMessage($"The user you are trying to make an {AdministratorRole} does not exist!");
                    return this.RedirectToAllUsers();
                }

                var result = await userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    this.AddSuccessMessage($"You made {user.Name} an {AdministratorRole}!");
                }
                else
                {
                    this.AddErrorMessage($"User {user.Name} is already in role {roleName}");
                }
            }
            else
            {
                this.AddErrorMessage($"The role does not exist!");
            }

            return this.RedirectToAllUsers();
        }

        public async Task<IActionResult> Deactivate(string id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.AddErrorMessage("The user you are trying to deactivate does not exist!");
                return this.RedirectToAllUsers();
            }

            this.admin.DeactivateUser(id);
            
            this.AddSuccessMessage($"You deactivated user {user.Name}!");

            return this.RedirectToAllUsers();
        }

        public async Task<IActionResult> Activate(string id)
        {
            string userId = this.userManager.GetUserId(this.User);
            var isDeleted = this.admin.IsDeleted(userId);

            if (isDeleted)
            {
                return BadRequest();
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.AddErrorMessage("The user you are trying to activate does not exist!");
                return this.RedirectToAllUsers();
            }

            this.admin.ActivateUser(id);

            this.AddSuccessMessage($"You activated user {user.Name}!");

            return this.RedirectToAllUsers();
        }
    }
}
