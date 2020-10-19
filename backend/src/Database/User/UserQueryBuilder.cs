using System.Text.Json;

namespace src.Database.User
{
    public class UserQueryBuilder
    {
        
        public static string GetDashboardQueryString(int dashboardId)
        {

            return $@"SELECT * FROM Dashboard WHERE Id={dashboardId};";
        }

        public static string CreateDashboardQueryString(string name, string description, JsonElement data)
        {
            return
                $@"INSERT INTO Dashboard (Name, Description, Data) 
                   VALUES ('{name}','{description}','{data}')
                   RETURNING Id";
        }

        public static string GetAllDashboardsQueryString()
        {
            return $@"SELECT * FROM Dashboard;";
        }

        public static string UpdateDashboardQueryString(int dashboardId, string name, string description, JsonElement jsonData)
        {
            return
                $@"UPDATE Dashboard SET Name = '{name}', Description = '{description}',Data = '{jsonData}' WHERE Id={dashboardId}";
        }

        public static string InsertOrUpdateDashboardQueryString(string name, string description, JsonElement jsonData)
        {
            return
                $@"INSERT INTO Dashboard (Name, Description, Data) 
                    VALUES ('{name}','{description}','{jsonData}')
                    ON CONFLICT (id) DO UPDATE 
                    SET Name = '{name}', Description = '{description}',Data = '{jsonData}'
                    RETURNING Id";
        }

        public static string InsertUserAccessToDashboardQueryString(string userId, int dashboardId, string accessLevel)
        {
            return 
                $@"INSERT INTO user_access_to_dashboard (user_id, dashboard_id, access_level) 
                    VALUES ('{userId}','{dashboardId}','{accessLevel}')";
        }
        public static string UpdateUserGroupAccessToDashboardQueryString(string userId, int dashboardId, string accessLevel)
        {
            return 
                $@"INSERT INTO user_group_access_to_dashboard (user_id, dashboard_id, access_level) 
                    VALUES ('{userId}','{dashboardId}','{accessLevel}')
                    ON CONFLICT (user_id) DO UPDATE 
                    SET user_id = '{userId}', dashboard_id = '{dashboardId}',access_level = '{accessLevel}'";
        }

        public static string GetUserDashboardQueryString(string userId)
        {
            return $@"SELECT * FROM dashboard INNER JOIN user_access_to_dashboard ON dashboard.id=user_access_to_dashboard.dashboard_id WHERE user_id='{userId}';";
        }

        public static string GetSharedUserDashboardQueryString(string userId)
        {
            return $@"SELECT * FROM dashboard INNER JOIN user__group_access_to_dashboard ON dashboard.id=user__group_access_to_dashboard.dashboard_id WHERE user_id='{userId}';";
        }
    }
}