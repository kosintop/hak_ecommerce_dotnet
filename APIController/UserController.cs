﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceDotNet.Models;
using eCommerceDotNet.Models.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceDotNet.APIController
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Route("add_admin")]
        public async Task<IActionResult> AddAdminUser([FromBody]AddAdminUserRequest request)
        {
            try
            {
                await AddUser(request.Username, request.Password, Role.Admin);
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("add_customer")]
        public async Task<IActionResult> AddCustomerUser([FromBody]AddCustomerUserRequest request)
        {
            try
            {
                await AddUser(request.Username, request.Password, Role.Customer);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("delete_user")]
        public async Task<IActionResult> DeleteUser([FromBody]DeleteUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        public async Task AddUser(string userName,string password,string roleName)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            ApplicationUser newUser = new ApplicationUser();
            newUser.UserName = userName;

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, roleName);
                await _userManager.UpdateAsync(newUser);
            }
            else
            {
                throw new Exception(result.Errors.ToString());
            }

        }

    }
}