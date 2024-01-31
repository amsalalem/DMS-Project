using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetSubCategoriesQueryHandler : IRequestHandler<GetSubCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public GetSubCategoriesQueryHandler(
           ICategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            var entity = await _categoryRepository.All.Where(c => c.ParentId == request.Id).ToListAsync();
            return _mapper.Map<List<CategoryDto>>(entity);
        }
    }
}
