using System.Text.Json;

namespace src.Database.User
{
    public class UserQueryBuilder
    {
        
        public static string GetDashboardQueryString(string userId, int dashboardId)
        {
            return 
                $@"SELECT * FROM dashboard AS D
                   INNER JOIN user_access_to_dashboard AS UA ON D.id=UA.dashboard_id 
                   WHERE user_id='{userId}' AND dashboard_id={dashboardId};";
        }

        public static string GetUserDashboardsQueryString(string userId)
        {
            return 
                $@"SELECT * FROM dashboard AS D
                   INNER JOIN user_access_to_dashboard AS UA ON D.id=UA.dashboard_id 
                   WHERE user_id='{userId}';";
        }
        public static string GetDashboardCellQueryString(string userId, int dashboardId, int cellId)
        {
            return 
                $@"SELECT * FROM cell AS C 
                   INNER JOIN dashboard AS D ON C.dashboard_id=D.id
                   INNER JOIN user_access_to_dashboard AS A ON C.dashboard_id=A.dashboard_id 
                   WHERE C.dashboard_id='{dashboardId}' AND A.user_id='{userId}' AND C.id={cellId};";
        }
        
        public static string GetDashboardCellsQueryString(string userId, int dashboardId)
        {
            return 
                $@"SELECT * FROM cell AS C 
                   INNER JOIN dashboard AS D ON C.dashboard_id=D.id
                   INNER JOIN user_access_to_dashboard AS UA ON C.dashboard_id=UA.dashboard_id 
                   WHERE C.dashboard_id='{dashboardId}' AND UA.user_id='{userId}';";
        }
        
        public static string CreateDashboardQueryString(string name, string description)
        {
            return
                $@"INSERT INTO Dashboard (Name, Description) 
                   VALUES ('{name}','{description}')
                   RETURNING Id";
        }

        public static string UpdateDashboardQueryString(string userId, int dashboardId, string name, string description)
        {
            return
                $@"UPDATE Dashboard AS D
                   SET Name = '{name}', Description = '{description}' 
                   FROM user_access_to_dashboard AS UA
                   WHERE D.Id={dashboardId} AND UA.user_id='{userId}'
                   RETURNING D.Id";

        }
        
        public static string CreateCellQueryString(int dashboardId, JsonElement options, JsonElement input)
        {
            return 
                $@"INSERT INTO Cell (dashboard_id, input, options) 
                   VALUES ({dashboardId},'{input}','{options}')
                   RETURNING Id";
        }

        public static string UpdateCellQueryString(string userId, int cellId, int dashboardId, JsonElement options, JsonElement inputQuery)
        {
            return 
                $@"UPDATE Cell AS C 
                   SET options = '{options}', input = '{inputQuery}' 
                   FROM user_access_to_dashboard AS UA 
                   WHERE C.Id={cellId} AND C.dashboard_id={dashboardId} AND UA.user_id='{userId}'
                   RETURNING C.Id";
        }


        public static string InsertUserAccessToDashboardQueryString(string userId, int dashboardId, string accessLevel)
        {
            return 
                $@"INSERT INTO user_access_to_dashboard (user_id, dashboard_id, access_level) 
                   VALUES ('{userId}','{dashboardId}','{accessLevel}')";
        }

        public static string GetSharedUserDashboardQueryString(string userId)
        {
            return 
                $@"SELECT * FROM dashboard AS D
                   INNER JOIN user__group_access_to_dashboard AS UA ON D.id=UA.dashboard_id 
                   WHERE user_id='{userId}';";
        }


        public static string DeleteDashboardQueryString(string userId, int dashboardId)
        {
            return 
                $@"DELETE FROM dashboard AS D 
                   USING user_access_to_dashboard AS UA 
                   WHERE D.id={dashboardId} AND UA.user_id='{userId}'";
        }
        
        public static string DeleteDashboardCellQueryString(string userId, int dashboardId, int cellId)
        {
            return 
                $@"DELETE FROM cell AS C 
                   USING user_access_to_dashboard AS UA 
                   WHERE C.id={cellId} AND C.dashboard_id={dashboardId} AND UA.user_id='{userId}'";
        }

        public static string DeleteUserAccessToDashboardQueryString(string userId, int dashboardId)
        {
            return 
                $@"DELETE FROM user_access_to_dashboard 
                   WHERE dashboard_id = {dashboardId} AND user_id='{userId}'";
        }
    }
}