using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries.Document;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.Document
{
    public class GetAllDocumentDropdownQueryHandler :  IRequestHandler<GetAllDocumentDropdownQuery, List<DocumentDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        public GetAllDocumentDropdownQueryHandler(IDocumentRepository documentRepository, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public async Task<List<DocumentDto>> Handle(GetAllDocumentDropdownQuery request, CancellationToken cancellationToken)
        {
            var entities = new List<Data.Entities.Document>();
            entities = await _documentRepository.All.ToListAsync();
            return _mapper.Map<List<DocumentDto>>(entities);
        }
    }
}
