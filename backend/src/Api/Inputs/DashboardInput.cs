using HotChocolate;

namespace src.Api.Inputs
{
    public class DashboardInput
    {
        
            [GraphQLNonNullType] 
            public string userId { get; set; }
            public int? dashboardId { get; set; }
            public string accessLevel { get; set; }
            public string name { get; set; }
            public string description { get; set; }
    }
}