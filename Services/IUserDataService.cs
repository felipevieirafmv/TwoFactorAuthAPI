using DTO;
using Model;
using System.Threading.Tasks;

namespace Services;

public interface IUserDataService
{
    Task Create(UserDataRegister data);
    Task<UserData> GetByLogin(string login);
}