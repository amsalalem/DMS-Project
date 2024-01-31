using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentTokenQueryHandler : IRequestHandler<GetDocumentTokenQuery, string>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public GetDocumentTokenQueryHandler(IDocumentTokenRepository documentTokenRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _documentTokenRepository = documentTokenRepository;
            _uow = uow;
        }
        public async Task<string> Handle(GetDocumentTokenQuery request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid();
            var documentToken = _documentTokenRepository.Find(request.Id);
            if (documentToken == null)
            {
                _documentTokenRepository.Add(new DocumentToken
                {
                    CreatedDate = DateTime.UtcNow,
                    DocumentId = request.Id,
                    Token = token
                });
                await _uow.SaveAsync();
            } 
            else
            {
                token = documentToken.Token;
            }
            return token.ToString();
        }
    }
}
