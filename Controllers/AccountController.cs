using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers {

    [Route("api/[controller]/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ResultVM> Register([FromBody] RegisterVM model) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");

                return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join("", errors)
                };
            }

            IdentityResult result = null;
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null) {
                return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>User already exists</li>"
                };
            }

            user = new AppUser {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email
            };

            result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                var resultErrors = result.Errors.Select(e => "<li>" + e.Description + "</li>");
                return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join("", resultErrors)
                };
            }

            return new ResultVM {
                Status = Status.Success,
                Message = "UserCreated",
                Data = user
            };
        }
    }
}