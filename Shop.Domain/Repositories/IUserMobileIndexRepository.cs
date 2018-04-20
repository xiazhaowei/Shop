using Shop.Domain.Models.Users;

namespace Shop.Domain.Repositories
{
    public interface IUserMobileIndexRepository
    {
        void Add(UserMobileIndex index);
        UserMobileIndex FindMobileIndex(string mobile);
    }
}
