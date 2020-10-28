using HotChocolate;

namespace src.Api.Inputs
{
    public class CellInput
    {
        [GraphQLNonNullType] public int dashboardId { get; set; }
        public int? cellId { get; set; }
        [GraphQLNonNullType] public CellOptionsInput options { get; set; }
        [GraphQLNonNullType] public CellGraphInput input { get; set; }
    }
}