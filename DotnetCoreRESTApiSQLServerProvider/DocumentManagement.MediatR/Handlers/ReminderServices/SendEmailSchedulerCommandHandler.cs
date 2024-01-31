using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class SendEmailSchedulerCommandHandler : IRequestHandler<SendEmailSchedulerCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly ILogger<SendEmailSchedulerCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly PathHelper _pathHelper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDocumentRepository _documentRepository;

        public SendEmailSchedulerCommandHandler(
            ISendEmailRepository sendEmailRepository,
            ILogger<SendEmailSchedulerCommandHandler> logger,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            PathHelper pathHelper,
            IMediator mediator,
            IDocumentRepository documentRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _pathHelper = pathHelper;
            _mediator = mediator;
            _documentRepository = documentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> Handle(SendEmailSchedulerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sendEmails = await _sendEmailRepository.All
                    .OrderByDescending(c => c.CreatedDate)
                    .Where(c => !c.IsSend)
                    .Take(10)
                    .ToListAsync();

                if (sendEmails.Count > 0)
                {
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    if (defaultSmtp == null)
                    {
                        return true;
                    }

                    foreach (var sendEmail in sendEmails)
                    {
                        if (!string.IsNullOrEmpty(sendEmail.Email))
                        {
                            try
                            {
                                var email = new SendEmailSpecification
                                {
                                    Body = sendEmail.Message,
                                    FromAddress = defaultSmtp.UserName,
                                    Host = defaultSmtp.Host,
                                    IsEnableSSL = defaultSmtp.IsEnableSSL,
                                    Password = defaultSmtp.Password,
                                    Port = defaultSmtp.Port,
                                    Subject = sendEmail.Subject,
                                    ToAddress = sendEmail.Email,
                                    CCAddress = "",
                                    UserName = defaultSmtp.UserName,
                                };

                                if (sendEmail.DocumentId != null)
                                {
                                    var document = await GetDocument(sendEmail.DocumentId.Value);
                                    var fileInfo = GetFileInfo(document);
                                    email.Attechments.Add(fileInfo);
                                }

                                EmailHelper.SendEmail(email);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message, ex);
                            }
                        }
                        sendEmail.FromEmail = defaultSmtp.UserName;
                        sendEmail.IsSend = true;
                    }

                    _sendEmailRepository.UpdateRange(sendEmails);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return true;
            }
        }


        private async Task<DocumentDto> GetDocument(Guid documentId)
        {
            var doc = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == documentId);
            return _mapper.Map<DocumentDto>(doc);
        }

        private Helper.FileInfo GetFileInfo(DocumentDto result)
        {

            string extension = Path.GetExtension(result.Url);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.DocumentPath, result.Url);

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
            var fileInfo = new Helper.FileInfo
            {
                Src = fileBytes,
                FileType = GetContentType(pathToSave),
                Extension = extension,
                Name = result.Name
            };
            return fileInfo;
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
    }
}
