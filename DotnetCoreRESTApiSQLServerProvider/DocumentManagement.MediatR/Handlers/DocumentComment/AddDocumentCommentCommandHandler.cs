using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentCommentCommandHandler : IRequestHandler<AddDocumentCommentCommand, ServiceResponse<DocumentCommentDto>>
    {
        private readonly IDocumentCommentRepository _documentCommentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddDocumentCommentCommandHandler> _logger;
        public AddDocumentCommentCommandHandler(
            IDocumentCommentRepository documentCommentRepository,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper,
            ILogger<AddDocumentCommentCommandHandler> logger)
        {
            _documentCommentRepository = documentCommentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<DocumentCommentDto>> Handle(AddDocumentCommentCommand request, CancellationToken cancellationToken)
        {
            var documentCommentEntity = _mapper.Map<DocumentComment>(request);
            _documentCommentRepository.Add(documentCommentEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<DocumentCommentDto>.Return500();
            }
            var documentCommentDto = _mapper.Map<DocumentCommentDto>(documentCommentEntity);
            return ServiceResponse<DocumentCommentDto>.ReturnResultWith200(documentCommentDto);
        }
    }
}
