using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers.V2
{
    //api/v2.0/test
    [Route("api/v{ver:apiVersion}/test/")]
    public class TestController : Controller
    {
        ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// Get a random object
        /// </summary>
        /// <returns></returns>
        /// <response code = "200">If get successful</response>
        /// <response code = "500">If server error</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_testService.TestFunction());
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

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
