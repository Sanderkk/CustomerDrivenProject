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
        Task<Cell> GetCell(string queryString);
        Task<List<Cell>> GetCells(string queryString);
        Task<int> UpdateDashboard(DashboardInput input);
        Task<bool> DeleteDashboard(string userId, int dashboardId);
        Task<int> UpdateCell(CellDataInput input);
        Task<bool> DeleteDashboardCell(string queryString);
    }
    
}