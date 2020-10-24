using System;
using System.Text.Json;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using src.Api.Inputs;
using src.Database;
using src.Database.User;

namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class DashboardMutation
    {
        

        public int UpdateDashboard(
            DashboardInput input,
            [Service] IUserRepository repo
        )
        {
            return repo.UpdateDashboard(input).Result;
        }

        public bool DeleteDashboard(
            int dashboardId,
            string userId,
            [Service] IUserRepository repo
        )
        {
            return repo.DeleteDashboard(userId, dashboardId).Result;
        }
        
        public Task<int> UpdateCell(
            CellDataInput input,
            [Service] IUserRepository repo
        )
        {
            return repo.UpdateCell(input);
        }

        public Task<bool> DeleteCell(
            [GraphQLNonNullType] string userId,
            [GraphQLNonNullType] int dashboardId,
            [GraphQLNonNullType] int cellId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.DeleteDashboardCellQueryString(userId, dashboardId, cellId);
            return repo.DeleteDashboardCell(queryString);
        }
    }
}