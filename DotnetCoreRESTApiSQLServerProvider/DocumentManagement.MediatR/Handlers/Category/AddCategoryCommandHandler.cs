using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
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
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public AddCategoryCommandHandler(
           ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _categoryRepository.FindBy(c => c.Name == request.Name && c.ParentId == request.ParentId).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new CategoryDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Category name already exist." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Category>(request);
            entity.Id = Guid.NewGuid();
            _categoryRepository.Add(entity);
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
