using DTO;
using Model;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class UserDataService : IUserDataService
{
    private TwoFactorAuthAPIDbContext ctx;
    private ISecurityService security;
    public UserDataService(TwoFactorAuthAPIDbContext ctx, ISecurityService security)
    {
        this.ctx = ctx;
        this.security = security;
    }

    public async Task Create(UserDataRegister data)
    {
        UserData user = new UserData();
        var salt = await security.GenerateSalt();

        user.UserName = data.UserName;
        user.UserPassword = await security.HashPassword(
            data.UserPassword, salt
        );
        user.Email = data.Email;
        user.Salt = salt;

        this.ctx.Add(user);
        await this.ctx.SaveChangesAsync();
    }

    public async Task<UserData> GetByLogin(string login)
    {
        var query =
            from u in this.ctx.UserData
            where u.UserName == login
            select u;
        
        return await query.FirstOrDefaultAsync();
    }
}