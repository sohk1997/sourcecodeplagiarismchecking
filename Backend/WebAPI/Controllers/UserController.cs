using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("changeStatus/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeUserStatus(int id)
        {
            _userService.ChangeUserStatus(id);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(new { UserList = _userService.GetAll()});
        }
    }
}