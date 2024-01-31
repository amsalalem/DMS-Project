using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public DeleteCategoryCommandHandler(
           ICategoryRepository categoryRepository,
           IDocumentRepository documentRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _categoryRepository = categoryRepository;
            _documentRepository = documentRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _categoryRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                var errorDto = new CategoryDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }

            var isExistingDoc = _documentRepository.All.Any(c => !c.IsDeleted && c.CategoryId == request.Id);

            if (isExistingDoc)
            {
                return new CategoryDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Category can not be deleted. Document is assign to this category." }
                };
            }

            _categoryRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new CategoryDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new CategoryDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Category deleted successfully." }
            };
        }
    }
}
