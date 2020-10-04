using System.Collections.Generic;
using System.Threading.Tasks;
using src.Api.Types;

namespace src.Database.User
{
    public interface IUserRepository
    {
        Task<Dashboard> GetDashboard(string queryString);
        Task<List<Dashboard>> GetDashboards(string queryString);
        Task<bool> UpdateDashboard(string userId,string accessLevel, string queryString);
        Task<bool> CreateDashboard(string queryString);
    }
    
}