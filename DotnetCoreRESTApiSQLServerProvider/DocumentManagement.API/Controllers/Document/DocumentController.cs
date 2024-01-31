using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.MediatR.Queries.Document;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DocumentManagement.API.Controllers
{
    /// <summary>
    /// Document
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class DocumentController : BaseController
    {
        public IMediator _mediator { get; set; }
        private IWebHostEnvironment _webHostEnvironment;
        private PathHelper _pathHelper;
        /// <summary>
        /// Document
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="pathHelper"></param>
        public DocumentController(
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;

        }
        /// <summary>
        /// Get Document By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Document/{id}", Name = "GetDocument")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            var getDocumentQuery = new GetDocumentQuery
            {
                Id = id
            };
            var response = await _mediator.Send(getDocumentQuery);
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response.Errors);
            }

            var result = response.Data;
            string extesion = Path.GetExtension(result.Url);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.DocumentPath, result.Url);
            //byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(pathToSave);

            byte[] fileBytes;
            using (var stream = new FileStream(pathToSave, FileMode.Open))
            {

                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                if (_pathHelper.AllowEncryption)
                {
                    fileBytes = AesOperation.DecryptStream(bytes, _pathHelper.AesEncryptionKey);
                }
                else
                {
                    fileBytes = bytes;
                }
            }

            result.ViewerType = DocumentType.GetDocumentType(extesion);
            if (extesion == ".pdf")
            {
                result.DocumentSource = Convert.ToBase64String(fileBytes);
            }
            else
            {
                result.DocumentSource = DocumentType.Get64ContentStartText(extesion) + Convert.ToBase64String(fileBytes);
            }
            result.ViewerType = DocumentType.GetDocumentType(extesion);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get Document By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Document/View/{id}", Name = "GetDocumentView")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
        public async Task<IActionResult> GetDocumentView(Guid id)
        {
            var getDocumentByIdQuery = new GetDocumentByIdQuery
            {
                Id = id
            };
            var response = await _mediator.Send(getDocumentByIdQuery);
            if (response == null)
            {
                return NotFound();
            }

            var result = response;
            string extesion = Path.GetExtension(result.Url);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.DocumentPath, result.Url);
            //byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(pathToSave);

            byte[] fileBytes;
            using (var stream = new FileStream(pathToSave, FileMode.Open))
            {

                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                if (_pathHelper.AllowEncryption)
                {
                    fileBytes = AesOperation.DecryptStream(bytes, _pathHelper.AesEncryptionKey);
                }
                else
                {
                    fileBytes = bytes;
                }
            }
            result.ViewerType = DocumentType.GetDocumentType(extesion);
            if (extesion == ".pdf")
            {
                result.DocumentSource = Convert.ToBase64String(fileBytes);
            }
            else
            {
                result.DocumentSource = DocumentType.Get64ContentStartText(extesion) + Convert.ToBase64String(fileBytes);
            }
            result.ViewerType = DocumentType.GetDocumentType(extesion);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get All Documents
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        [HttpGet("Documents")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
        public async Task<IActionResult> GetDocuments([FromQuery] DocumentResource documentResource)
        {
            var getAllDocumentQuery = new GetAllDocumentQuery
            {
                DocumentResource = documentResource
            };
            var result = await _mediator.Send(getAllDocumentQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }

        [HttpGet("Documents/Approval")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
        public async Task<IActionResult> GetApprovalDocuments([FromQuery] DocumentResource documentResource)
        {
            var getAllDocumentQuery = new GetAllDocumentQuery
            {
                DocumentResource = documentResource
            };
            var result = await _mediator.Send(getAllDocumentQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }

        /// <summary>
        /// Create a document.
        /// </summary>
        /// <param name="addDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("Document")]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
        public async Task<IActionResult> AddDocument([FromForm] AddDocumentCommand addDocumentCommand)
        {
            var result = await _mediator.Send(addDocumentCommand);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
        }

        [HttpPost("Document/assign")]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
        public async Task<IActionResult> AddDocumentToMe([FromForm] AddDocumentToMeCommand addDocumentCommand)
        {
            var result = await _mediator.Send(addDocumentCommand);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
        }
        /// <summary>
        /// Document by upload
        /// </summary>
        /// <returns></returns>
        [HttpPost("Document/upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string newFileName = "";
                Guid Id = Guid.NewGuid();
                string documentPath = _pathHelper.DocumentPath;
                string contentRootpath = _webHostEnvironment.ContentRootPath;
                string newPath = Path.Combine(contentRootpath, documentPath);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extesion = Path.GetExtension(fileName);
                    newFileName = Id + extesion;
                    string fullPath = Path.Combine(newPath, newFileName);
                    var bytesData = AesOperation.ReadAsBytesAsync(file);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        if (_pathHelper.AllowEncryption)
                        {
                            var byteArray = AesOperation.EncryptStream(bytesData, _pathHelper.AesEncryptionKey);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        else
                        {
                            stream.Write(bytesData, 0, bytesData.Length);
                        }

                    }
                    var fileInfo = await Task.FromResult(new
                    {
                        FileName = newFileName,
                        Id = Id
                    });
                    return Ok(fileInfo);
                }
                else
                {
                    return BadRequest("File not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Upload document.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateDocumentCommand"></param>
        /// <returns></returns>
        [HttpPut("Document/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
        public async Task<IActionResult> UpdateDocument(Guid Id, UpdateDocumentCommand updateDocumentCommand)
        {
            updateDocumentCommand.Id = Id;
            var result = await _mediator.Send(updateDocumentCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Delete Document.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(Guid Id)
        {
            var deleteDocumentCommand = new DeleteDocumentCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteDocumentCommand);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Document/{id}/download")]
        public async Task<IActionResult> DownloadDocument(Guid id, bool isVersion)
        {
            var commnad = new DownloadDocumentCommand
            {
                Id = id,
                IsVersion = isVersion
            };

            var path = await _mediator.Send(commnad);
            path = Path.Combine(_webHostEnvironment.ContentRootPath, path);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            byte[] newBytes;
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                if (_pathHelper.AllowEncryption)
                {
                    newBytes = AesOperation.DecryptStream(bytes, _pathHelper.AesEncryptionKey);
                }
                else
                {
                    newBytes = bytes;
                }
            }
            return File(newBytes, GetContentType(filePath), filePath);
        }

        [HttpGet("Document/{id}/officeviewer")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDocumentFileByToken(Guid id, Guid token, bool isVersion)
        {
            var deleteDocumentTokenCommand = new GetDocumentPathByTokenCommand
            {
                Id = id,
                Token = token
            };
            var result = await _mediator.Send(deleteDocumentTokenCommand);
            if (!result)
            {
                return NotFound();
            }
            var commnad = new DownloadDocumentCommand
            {
                Id = id,
                IsVersion = isVersion,
            };

            var path = await _mediator.Send(commnad);
            path = Path.Combine(_webHostEnvironment.ContentRootPath, path);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            byte[] newBytes;
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                if (_pathHelper.AllowEncryption)
                {
                    newBytes = AesOperation.DecryptStream(bytes, _pathHelper.AesEncryptionKey);
                }
                else
                {
                    newBytes = bytes;
                }
            }
            return File(newBytes, GetContentType(filePath), filePath);
        }


        [HttpGet("Document/{id}/isDownloadFlag")]
        [AllowAnonymous]
        public async Task<ActionResult> GetIsDownloadFlag(Guid id)
        {
            var deleteDocumentTokenCommand = new GetIsDownloadFlagQuery
            {
                DocumentId = id,
            };
            var result = await _mediator.Send(deleteDocumentTokenCommand);
            return Ok(result);
        }

        /// <summary>
        /// Read text Document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isVersion"></param>
        /// <returns></returns>
        [HttpGet("Document/{id}/readText")]
        public async Task<IActionResult> ReadTextDocument(Guid id, bool isVersion)
        {
            var commnad = new DownloadDocumentCommand
            {
                Id = id,
                IsVersion = isVersion
            };
            var path = await _mediator.Send(commnad);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            byte[] newBytes;
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
                var latestBytes = memory.ToArray();
                if (_pathHelper.AllowEncryption)
                {
                    newBytes = AesOperation.DecryptStream(latestBytes, _pathHelper.AesEncryptionKey);
                }
                else
                {
                    newBytes = latestBytes;
                }
            }
            string utfString = Encoding.UTF8.GetString(newBytes, 0, newBytes.Length);
            // var result = System.IO.File.ReadAllLines(filePath);
            return Ok(new { result = new string[] { utfString } });
        }

        [HttpGet("Document/{id}/getMetatag")]
        public async Task<IActionResult> GetDocumentMetatags(Guid id)
        {
            var commnad = new GetDocumentMetaDataByIdQuery
            {
                DocumentId = id,
            };
            var documentMetas = await _mediator.Send(commnad);
            return Ok(documentMetas);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet("Document/dropdown")]
        [Produces("application/json", "application/xml", Type = typeof(List<DocumentDto>))]
        public async Task<IActionResult> GetAllDocumentsForDropDown()
        {
            var getAllDocumentQuery = new GetAllDocumentDropdownQuery { IsAuthirized = false };
            var result = await _mediator.Send(getAllDocumentQuery);
            return Ok(result);
        }
    }
}
