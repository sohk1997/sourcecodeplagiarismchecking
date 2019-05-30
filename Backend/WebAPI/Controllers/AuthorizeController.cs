using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Root.Model;
using Service.Services;
using ViewModel.ViewModel;
using WebAPI.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers {
    [AllowAnonymous]
    [Route ("token")]
    public class AuthorizeController : Controller {
        private UserManager<User> _userManager;
        private ILogger<AuthorizeController> _logger;
        private IFeatureService _featureService;

        private IRoleStore<IdentityRole> _roleStore;
        public AuthorizeController (UserManager<User> userManager, ILogger<AuthorizeController> logger,
            IFeatureService featureService,
            IRoleStore<IdentityRole> roleStore) {
            _logger = logger;
            _userManager = userManager;
            _roleStore = roleStore;
            _featureService = featureService;
        }

        // GET: api/<controller>

        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

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
        [ProducesResponseType (200, Type = typeof (object))]
        [ProducesResponseType (400, Type = typeof (string))]
        [ProducesResponseType (500)]
        public async Task<IActionResult> Post ([FromBody] LoginViewModel user) {
            var rootUser = await _userManager.FindByNameAsync (user.Username);
            var login = await _userManager.CheckPasswordAsync (rootUser, user.Password);
            if (login) {
                var roleList = _featureService.Get (rootUser.RoleId);
                var tokenString = rootUser.BuildToken (roleList);
                return Ok (new { tokenString , role = roleList});
            } else {
                _logger.LogError ("Wrong user name or password " + user.Username );
                return new BadRequestResult();
            }
        }

        // PUT api/<controller>/5
        [HttpPut ("")]
        public async Task<IActionResult> Put ([FromBody] LoginViewModel user) {
            var roleId = (await _roleStore.FindByNameAsync (user.Role, CancellationToken.None)).Id;
            var result = await _userManager.CreateAsync (new User () {
                Id = Guid.NewGuid ().ToString (),
                    UserName = user.Username,
                    RoleId = roleId,
                    RoleName = "random",
            }, user.Password);
            return Ok (result);

        }

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}