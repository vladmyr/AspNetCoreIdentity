using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<ResultVM> Login([FromBody] LoginVM model) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");
                return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join("", errors)
                };
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !isPasswordValid) {
                return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Invalid Username or Passowrd</li>"
                };
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(identity)
            );

            return new ResultVM {
                Status = Status.Success,
                Message = "Successfull login",
                Data = model
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<UserClaims> Claims() {
            var claims = User.Claims.Select(c => new ClaimVM {
                Type = c.Type,
                Value = c.Value
            });

            return new UserClaims {
                UserName = User.Identity.Name,
                Claims = claims
            };
        }

        [HttpGet]
        public async Task<UserStateVM> Authenticated() {
            return new UserStateVM {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Username = User.Identity.IsAuthenticated
                    ? User.Identity.Name
                    : string.Empty
            };
        }

        [HttpPost]
        public async Task SignOut() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}