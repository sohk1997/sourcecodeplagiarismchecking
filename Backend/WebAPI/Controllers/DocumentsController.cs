﻿using System;
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
    [Route("api/submission")]
    public class DocumentsController : Controller
    {
        private IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Get list of Submission
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ReturnDocumentViewModel), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [Authorize]
        public IActionResult Get([FromQuery]int start = 0, [FromQuery]int length = 10, string _ = "", int draw = 1)
        {
            string idInString = this.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            var id = int.Parse(idInString);
            var result = _documentService.GetAll(id, start, length);
            result.Draw = draw + 1;
            return Ok(result);
        }

        /// <summary>
        /// Get list of Submission for admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin")]
        [ProducesResponseType(typeof(ReturnDocumentViewModel), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetForAdmin([FromQuery]int start = 0, [FromQuery]int length = 10, string _ = "", int draw = 1)
        {
            var result = _documentService.GetAll(start, length);
            result.Draw = draw + 1;
            return Ok(result);
        }

        /// <summary>
        /// Get check result of a submission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/result")]
        //[Authorize(Roles = ("DOCUMENT_V"))]
        [ProducesResponseType(typeof(ResponseResult), 200)]
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
        /// Upload new submission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Upload new submission
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        [HttpPost("multi")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostMulti(UploadMultiRequest requests)
        {
            string idInString = this.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var userId = int.Parse(idInString);
            foreach (var file in requests.Files)
            {
                int id = await _documentService.UploadToCloud(file, false, false, userId);
            }
            return Ok();
        }

    }
}