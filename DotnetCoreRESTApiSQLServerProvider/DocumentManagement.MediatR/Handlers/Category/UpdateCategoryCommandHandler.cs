using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdateCategoryCommandHandler(
           ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _categoryRepository
                .FindBy(c => c.Name == request.Name && c.Id != request.Id && c.ParentId == request.ParentId).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new CategoryDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Category Name already exist for another category." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Category>(request);
            _categoryRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new CategoryDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<CategoryDto>(entity);
            return entityDto;
        }
    }
}
