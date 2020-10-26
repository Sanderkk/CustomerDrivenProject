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

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class DashboardQuery
    {
        public Dashboard GetDashboard(
            [GraphQLNonNullType] string userId,
            [GraphQLNonNullType] int dashboardId,
            [Service] IUserRepository repo
        )
        {
            Console.WriteLine("ok");
            string queryString = UserQueryBuilder.GetDashboardQueryString(userId, dashboardId);
            return repo.GetDashboard(queryString).Result;
        }
        
        public List<Dashboard> GetDashboards(
            [GraphQLNonNullType] string userId,
            [CurrentUserGlobalState] CurrentUser user,
            [Service] IUserRepository repo
        )
        {
            Console.WriteLine("Dashboard query:");
            Console.WriteLine(user);
            string queryString = UserQueryBuilder.GetUserDashboardsQueryString(userId);
            return repo.GetDashboards(queryString).Result;
        }
        
        public Cell GetCell(
            [GraphQLNonNullType] string userId,
            [GraphQLNonNullType] int dashboardId,
            [GraphQLNonNullType] int cellId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardCellQueryString(userId, dashboardId, cellId);
            return repo.GetCell(queryString).Result;
        }
        
        public List<Cell> GetCells(
            [GraphQLNonNullType] string userId,
            [GraphQLNonNullType] int dashboardId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardCellsQueryString(userId, dashboardId);
            return repo.GetCells(queryString).Result;
        }

        public List<Dashboard> GetSharedUserDashboards(
            [GraphQLNonNullType] string userId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetSharedUserDashboardQueryString(userId);
            return repo.GetDashboards(queryString).Result;
        }
    }
}