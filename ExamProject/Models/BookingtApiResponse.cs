namespace ExamProject.Models
{
    public class BookTicketResponse
    {
        public decimal PriceSummary { get; set; }
        public List<TicketCategorySummary> TicketsPerCategories { get; set; } = new();
    }

    public class TicketCategorySummary
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal SummaryPrice { get; set; }
        public List<TicketDetailsResponse> Tickets { get; set; } = new();
    }

    public class TicketDetailsResponse
    {
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
