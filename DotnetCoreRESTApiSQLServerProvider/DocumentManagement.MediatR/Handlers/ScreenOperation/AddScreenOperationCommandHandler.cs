using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddScreenOperationCommandHandler : IRequestHandler<AddScreenOperationCommand, ScreenOperationDto>
    {
        private readonly IScreenOperationRepository _screenOperationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public AddScreenOperationCommandHandler(
           IScreenOperationRepository screenOperationRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _screenOperationRepository = screenOperationRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ScreenOperationDto> Handle(AddScreenOperationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _screenOperationRepository.FindBy(c => c.ScreenId == request.ScreenId && c.OperationId == request.OperationId).FirstOrDefaultAsync();
            if (entity == null)
            {
                entity = _mapper.Map<ScreenOperation>(request);
                entity.Id = Guid.NewGuid();
                _screenOperationRepository.Add(entity);
                if (await _uow.SaveAsync() <= 0)
                {
                    var errorDto = new ScreenOperationDto
                    {
                        StatusCode = 500,
                        Messages = new List<string> { "An unexpected fault happened. Try again later." }
                    };
                    return errorDto;
                }
            }
            return _mapper.Map<ScreenOperationDto>(entity);
        }
    }
}
