using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserAuthDto> BuildUserAuthObject(User appUser);
    }
}
