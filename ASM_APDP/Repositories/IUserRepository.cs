using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User getUserById(int id);

        int Create(User user);
        int Update(User user);

        int Delete(int id);
    }
}
