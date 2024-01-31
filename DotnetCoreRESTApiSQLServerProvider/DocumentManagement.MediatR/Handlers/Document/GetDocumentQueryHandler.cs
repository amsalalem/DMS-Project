using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, ServiceResponse<DocumentDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private readonly IUserNotificationRepository _userNotificationRepository;
        public GetDocumentQueryHandler(
           IDocumentRepository documentRepository,
             IMapper mapper,
             IUserNotificationRepository userNotificationRepository
            )
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
            _userNotificationRepository = userNotificationRepository;
        }
        public async Task<ServiceResponse<DocumentDto>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var entity = await _documentRepository.GetDocumentById(request.Id);
            // mark notification as read.
            await _userNotificationRepository.MarkAsReadByDocumentId(request.Id);
            if (entity != null)
                return ServiceResponse<DocumentDto>.ReturnResultWith200(entity);
            else
                return ServiceResponse<DocumentDto>.ReturnFailed(404, "Document is not found.");
        }
    }
}
