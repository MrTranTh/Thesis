namespace Thesis.Models
{
    public class ThongKeViewModel
    {
        public Dictionary<DateTime, int> AccessCountByDay { get; set; }
        public Dictionary<string, int> AccessCountByMonth { get; set; }
        public Dictionary<int, int> AccessCountByYear { get; set; }
    }
}
