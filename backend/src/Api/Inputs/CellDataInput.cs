using HotChocolate;

namespace src.Api.Inputs
{
    public class CellDataInput
    {
        [GraphQLNonNullType] public string UserId { get; set; }
        [GraphQLNonNullType] public int dashboardId { get; set; }
        public int? cellId { get; set; }
        [GraphQLNonNullType] public string options { get; set; }
        [GraphQLNonNullType] public string input { get; set; }
    }
}