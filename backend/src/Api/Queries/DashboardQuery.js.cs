using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;
using src.Api.Models;
using src.Database.User;
using HotChocolate.AspNetCore.Authorization;

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class DashboardQuery
    {
        [Authorize(Policy = "Default")]
        public Dashboard GetDashboard(
            [GraphQLNonNullType] int dashboardId,
            [CurrentUserGlobalState] CurrentUser user,
            //[CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardQueryString(user.UserId, dashboardId);
            return repo.GetDashboard(queryString).Result;
        }
        
        [Authorize(Policy = "Default")]
        public List<Dashboard> GetDashboards(
            [Service] IUserRepository repo,
            [CurrentUserGlobalState] CurrentUser user
        )
        {
            string queryString = UserQueryBuilder.GetUserDashboardsQueryString(user.UserId);
            return repo.GetDashboards(queryString).Result;
        }
        
        [Authorize(Policy = "Default")]
        public Cell GetCell(
            [GraphQLNonNullType] int dashboardId,
            [GraphQLNonNullType] int cellId,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardCellQueryString(user.UserId, dashboardId, cellId);
            return repo.GetCell(queryString).Result;
        }
        
        [Authorize(Policy = "Default")]
        public List<Cell> GetCells(
            [GraphQLNonNullType] int dashboardId,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardCellsQueryString(user.UserId, dashboardId);
            return repo.GetCells(queryString).Result;
        }

        [Authorize(Policy = "Default")]
        public List<Dashboard> GetSharedUserDashboards(
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetSharedUserDashboardQueryString(user.UserId);
            return repo.GetDashboards(queryString).Result;
        }
    }
}