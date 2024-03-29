﻿using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteDocumentTokenCommandHandler : IRequestHandler<DeleteDocumentTokenCommand, bool>
    {

        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public DeleteDocumentTokenCommandHandler(IDocumentTokenRepository documentTokenRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _documentTokenRepository = documentTokenRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteDocumentTokenCommand request, CancellationToken cancellationToken)
        {
            var documentToken = _documentTokenRepository.All.FirstOrDefault(c => c.Token == request.Token);
            if (documentToken != null)
            {
                _documentTokenRepository.Remove(documentToken);
                await _uow.SaveAsync();
            }
            return true;
        }
    }
}
