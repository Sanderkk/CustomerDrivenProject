using System.Text.Json;

namespace src.Api.Types
{
    public class Dashboard
    {
        public int DashboardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string  Data { get; set; } }
}