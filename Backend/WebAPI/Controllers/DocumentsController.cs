using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Root.Data;
using Root.Model;
using Service.Services;
using ViewModel.Document;
using WebAPI.ViewModel;

namespace WebAPI.Controllers
{
    //[Authorize]
    [Route("api/document")]
    public class DocumentsController : Controller
    {
        private IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Get list of Document
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DocumentInList>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [Authorize]
        public IActionResult Get()
        {
            string idInString = this.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            var id = int.Parse(idInString);
            return Ok(_documentService.GetAll(id));
        }

        /// <summary>
        /// Get document have id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{id}/result")]
        //[Authorize(Roles = ("DOCUMENT_V"))]
        [ProducesResponseType(typeof(DocumentInfo), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var document = _documentService.GetResult(id);
            if (document != null)
            {
                return Ok(document);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Upload new document
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IFormFile), 201)]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post(CheckRequest request)
        {
            string idInString = this.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            System.Console.WriteLine(idInString);
            var userId = int.Parse(idInString);
            var file = request.File;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value));
            }
            if (file == null || file.Length < 0)
            {
                NotFound();
            }
            else
            {                
                int id = await _documentService.UploadToCloud(file, request.WebCheck, request.PeerCheck, userId);
                return Ok();
            }
            return NotFound();
        }        
    }
}