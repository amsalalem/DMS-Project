using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteScreenOperationCommandHandler : IRequestHandler<DeleteScreenOperationCommand, ScreenOperationDto>
    {
        private readonly IScreenOperationRepository _screenOperationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public DeleteScreenOperationCommandHandler(
           IScreenOperationRepository screenOperationRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _screenOperationRepository = screenOperationRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ScreenOperationDto> Handle(DeleteScreenOperationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _screenOperationRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (entityExist == null)
            {
                var errorDto = new ScreenOperationDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found Role" }
                };
                return errorDto;
            }
            _screenOperationRepository.Remove(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new ScreenOperationDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new ScreenOperationDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Screen Operation deleted successfully." }
            };
        }
    }
}
