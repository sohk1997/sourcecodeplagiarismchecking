using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Service.Services;
using ViewModel.Customer;
using WebAPI.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers {
    [Authorize]
    [Route ("api/customer")]
    public class CustomerController : Controller {
        private ICustomerService _customerService;

        public CustomerController (ICustomerService customerService) {
            _customerService = customerService;
        }

        /// <summary>
        /// Get list of Customer have name contain 'a'
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Authorize(Roles = ("CUSTOMER_V"))]
        [ProducesResponseType (typeof (IEnumerable<CustomerInList>), 200)]
        [ProducesResponseType (typeof (string), 500)]
        public IActionResult Get () {
            return Ok (new { ReturnList = _customerService.CustomerProcedure(1,"") });
        }

        /// <summary>
        /// Get customer have id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet ("{id}")]
        [Authorize(Roles = ("CUSTOMER_V"))]
        [ProducesResponseType (typeof (CustomerInfo), 200)]
        [ProducesResponseType (404)]
        [ProducesResponseType (500)]
        public IActionResult Get (int id) {
            var customer = _customerService.Get (id);
            if (customer != null) {
                return Ok (customer);
            } else {
                return NotFound ();
            }
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="vmCustomer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = ("CUSTOMER_C"))]
        [ProducesResponseType (typeof (CustomerCreate), 201)]
        [ProducesResponseType (400, Type = typeof (ModelStateDictionary))]
        [ProducesResponseType (500)]
        public IActionResult Post ([FromBody]CustomerCreate vmCustomer) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState.Where (x => x.Value.Errors.Count > 0).ToDictionary (x => x.Key, x => x.Value));
            }
            int id = _customerService.Create (vmCustomer);
            string location = Request.HttpContext.Request.Host + "/api/customer/" + id;
            return Created (location, vmCustomer);
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="vmCustomer"></param>
        /// <returns></returns>
        // PUT api/<controller>/5
        [HttpPut]
        [Authorize(Roles = ("CUSTOMER_U"))]
        [ProducesResponseType (200)]
        [ProducesResponseType (400, Type = typeof (ModelStateDictionary))]
        [ProducesResponseType (500)]
        public async Task<IActionResult> Put ([FromBody]CustomerUpdate vmCustomer) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState.Where (x => x.Value.Errors.Count > 0).ToDictionary (x => x.Key, x => x.Value));
            }
            _customerService.Update (vmCustomer);
            return Ok ();
        }

        /// <summary>
        /// Delete customer with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpDelete ("{id}")]
        [Authorize(Roles = ("CUSTOMER_D"))]
        [ProducesResponseType (200)]
        [ProducesResponseType (500)]
        public async Task<IActionResult> Delete (int id) {
            _customerService.Delete (id);
            return Ok ();
        }
    }
}