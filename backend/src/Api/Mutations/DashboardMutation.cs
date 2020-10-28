using System;
using System.Text.Json;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using src.Api.Inputs;
using src.Api.Models;
using src.Database;
using src.Database.User;

namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class DashboardMutation
    {
        
        [Authorize(Policy = "Default")]
        public int UpdateDashboard(
            DashboardInput input,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            return repo.UpdateDashboard(user.UserId, input).Result;
        }

        [Authorize(Policy = "Default")]
        public bool DeleteDashboard(
            int dashboardId,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            return repo.DeleteDashboard(user.UserId, dashboardId).Result;
        }
        
        [Authorize(Policy = "Default")]
        public Task<int> UpdateCell(
            CellInput input,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            return repo.UpdateCell(user.UserId, input);
        }

        [Authorize(Policy = "Default")]
        public Task<bool> DeleteCell(
            [GraphQLNonNullType] int dashboardId,
            [GraphQLNonNullType] int cellId,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.DeleteDashboardCellQueryString(user.UserId, dashboardId, cellId);
            return repo.DeleteDashboardCell(queryString);
        }
    }
}