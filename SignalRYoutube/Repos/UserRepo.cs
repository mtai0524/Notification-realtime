using Microsoft.EntityFrameworkCore;
using SignalRYoutube.Data;
using SignalRYoutube.Models;

namespace SignalRYoutube.Repos
{
    public class UserRepo
    {
        private readonly ApplicationDBContext dbContext;   

        public UserRepo(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TblUser> GetUserDetails(string username, string password)
        {
            return await dbContext.TblUser.FirstOrDefaultAsync(user => user.Username == username && user.Password == password);
        }
    }
}
