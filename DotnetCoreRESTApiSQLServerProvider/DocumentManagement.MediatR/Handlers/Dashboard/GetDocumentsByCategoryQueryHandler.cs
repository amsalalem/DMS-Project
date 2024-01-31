using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentsByCategoryQueryHandler : IRequestHandler<GetDocumentsByCategoryQuery, List<DocumentByCategory>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;


        public GetDocumentsByCategoryQueryHandler(
            IDocumentRepository documentRepository,
            IUserRepository userRepository,
            UserInfoToken userInfoToken,
            ICategoryRepository categoryRepository)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<DocumentByCategory>> Handle(GetDocumentsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;
            var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
            var documentsQuery = _documentRepository.AllIncluding(c => c.User, c => c.DocumentRolePermissions, c => c.DocumentUserPermissions)
                                        .Where(d => (d.DocumentUserPermissions.Any(c => c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))
                                                    || (d.DocumentRolePermissions.Any(c => userRoles.Contains(c.RoleId)
                                                    && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today))))));

            var documentCount = documentsQuery
                .GroupBy(c => c.CategoryId)
                .Select(cs => new
                {
                    CategoryId = cs.Key,
                    DocumentCount = cs.Count()
                }).ToList();

            var categories = await _categoryRepository.All.ToListAsync();

            var result = (from c in categories
                          join p in documentCount on c.Id equals p.CategoryId into ps
                          from p in ps.DefaultIfEmpty()
                          select new DocumentByCategory { CategoryName = c.Name, DocumentCount = p == null ? 0 : p.DocumentCount }).ToList();

            return result;
        }
    }
}
