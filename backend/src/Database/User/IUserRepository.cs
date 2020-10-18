using System.Collections.Generic;
using System.Threading.Tasks;
using src.Api.Inputs;
using src.Api.Types;

namespace src.Database.User
{
    public interface IUserRepository
    {
        Task<Dashboard> GetDashboard(string queryString);
        Task<List<Dashboard>> GetDashboards(string queryString);
        Task<bool> UpdateDashboard(DashboardInput input);
        Task<bool> CreateDashboard(string queryString);
        Task<bool> DeleteDashboard(int dashboardId);
    }
    
}