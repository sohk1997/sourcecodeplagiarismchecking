using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Root.CommonEnum;
using Root.Model;
using Service.Services;
using ViewModel.ViewModel;
using WebAPI.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("token")]
    public class AuthorizeController : Controller
    {
        private IUserService _userService;

        public AuthorizeController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/<controller>
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code = "200">If login successful, return an object have tokenString</response>
        /// <response code = "400">If login fail, return a error message</response>
        /// <response code = "500">If internal server error</response>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] LoginViewModel user)
        {
            var authorizeUser = _userService.GetUser(user);
            if (authorizeUser != null)
            {
                var roleList = new List<string>();
                if (authorizeUser.RoleId == (int)RoleType.ADMIN)
                {
                    roleList.Add("Admin");
                }
                else
                {
                    roleList.Add("User");
                }

                var tokenString = authorizeUser.BuildToken(roleList);
                return Ok(new { token = tokenString, role = roleList });

            }
            else
            {
                return new BadRequestResult();
            }
        }

        // PUT api/<controller>/5
        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody] LoginViewModel user)
        {
            _userService.Create(user);
            return Ok();

        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> CheckToken([FromQuery]string role = "User")
        {
            if (User.IsInRole(role))
            {
                return Ok();
            }
            return Unauthorized();
        }


    }
}