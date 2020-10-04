using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;
using src.Database.User;

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class DashboardQuery
    {
        public Dashboard GetDashboard(
            [GraphQLNonNullType] int dashboardId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetDashboardQueryString(dashboardId);
            return repo.GetDashboard(queryString).Result;
        }
        
        public List<Dashboard> GetUserDashboards(
            [GraphQLNonNullType] string userId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetUserDashboardQueryString(userId);
            return repo.GetDashboards(queryString).Result;
        }

        public List<Dashboard> GetSharedUserDashboards(
            [GraphQLNonNullType] string userId,
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetSharedUserDashboardQueryString(userId);
            return repo.GetDashboards(queryString).Result;
        }

        public List<Dashboard> GetAllDashboards(
            [Service] IUserRepository repo
        )
        {
            string queryString = UserQueryBuilder.GetAllDashboardsQueryString();
            return repo.GetDashboards(queryString).Result;
        }


    }
}