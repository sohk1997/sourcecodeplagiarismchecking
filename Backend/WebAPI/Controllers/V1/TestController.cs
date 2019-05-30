using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Service.Services;
using ViewModel.ViewModel;

namespace WebAPI.Controllers.V1
{
    //api/v-1.0/test
    [Authorize]
    [Route("api/v-{ver:apiVersion}/Test/")]
    public class TestController : Controller
    {
        private ITestService _testService;
        private ILogger<TestController> _logger;

        public TestController(ITestService testService, ILogger<TestController> logger)
        {
            _testService = testService;
            _logger = logger;
        }



        /// <summary>
        /// Get a list of test object where Id > 2
        /// </summary>
        /// <remarks>
        /// GET /Test
        /// </remarks>
        /// <returns>A list of test object</returns>
        /// <response code = "200">if list can get</response>
        /// <response code = "401">if unauthorize</response>
        /// <response code = "500">if internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TestViewModel>),200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            _logger.LogError("Error");
            return Ok(_testService.TestProcedure());
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        /// <summary>
        /// Check if the model is valid or not.
        /// Just user "luatluat" can use
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code = "200">If model is valid</response>
        /// <response code = "400">If model is invalid</response>
        /// <response code = "401">If unauthorize</response>
        /// <response code = "403">If user is not allowed to access the control</response>
        [HttpPost]
        [Authorize(Policy = "Luat")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Dictionary<string,List<string>>),400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public IActionResult Post([FromBody]TestValidationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            }
            return Ok("The model is valid");
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
