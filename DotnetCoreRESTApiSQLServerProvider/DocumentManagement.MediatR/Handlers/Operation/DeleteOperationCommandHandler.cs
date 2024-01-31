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
    public class DeleteOperationCommandHandler : IRequestHandler<DeleteOperationCommand, OperationDto>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        public DeleteOperationCommandHandler(
           IOperationRepository operationRepository,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _operationRepository = operationRepository;
            _uow = uow;
        }

        public async Task<OperationDto> Handle(DeleteOperationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _operationRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                var errorDto = new OperationDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }

            _operationRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new OperationDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new OperationDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Operation deleted successfully." }
            };
        }
    }
}
