using src.Api.Inputs;

namespace src.Api.Types
{
    public class Cell
    {
        public int CellId { get; set; }
        public int DashboardId { get; set; }
        public CellOptions Options { get; set; }
        public CellGraphData Input { get; set; } 
    }
}