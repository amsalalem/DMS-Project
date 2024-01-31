using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, DocumentDto>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public DeleteDocumentCommandHandler(
           IDocumentRepository documentRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<DocumentDto> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _documentRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                var errorDto = new DocumentDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }

            _documentRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new DocumentDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new DocumentDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Document deleted successfully." }
            };
        }
    }
}
