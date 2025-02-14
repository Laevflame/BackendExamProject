using ExamProject.Entities;

namespace ExamProject.Models
{
    public class RevokeTicketDetails
    {
        public string BookedTicketDetailId { get; set; } = string.Empty;
        public string BookedTicketId { get; set; } = string.Empty;
        public string TicketId { get; set; } = string.Empty;
        public int BookedTicketDetailsQuantity { get; set; }
        public RevokeTicketGet? Ticket { get; set; }
    }
    public class RevokeTicketGet
    {
        public string TicketId { get; set; } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
    public class RevokeBookedTicket
    {
        public string BookedTicketId { get; set; } = string.Empty;
    }
}
