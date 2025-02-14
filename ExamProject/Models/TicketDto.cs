namespace ExamProject.Models
{
    public class TicketDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TicketRemainingQuota { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public decimal TicketPrice { get; set; }
        public DateTime EventDate { get; set; }
    }
}
