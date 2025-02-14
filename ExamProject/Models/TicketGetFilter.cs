namespace ExamProject.Models
{
    public class TicketGetFilter
    {
        public string? CategoryName { get; set; }
        public string? TicketCode { get; set; }
        public string? TicketName { get; set; }
        public decimal? TicketPrice { get; set; }
        public DateTime? EventDate { get; set; }
        public string? OrderBy { get; set; } = "TicketCode"; 
        public string? OrderState { get; set; } = "asc";
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 10;
    }
}
