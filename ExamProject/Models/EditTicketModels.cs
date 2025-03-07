namespace ExamProject.Models
{
    public class EditBookedTicket
    {
        public string BookedTicketId { get; set; } = string.Empty;
        public List<EditTicketDetail> BookedTicketDetails { get; set; } = new();
    }

    public class EditTicketDetail
    {
        public string BookedTicketDetailId { get; set; } = string.Empty;
        public string BookedTicketId { get; set; } = string.Empty;
        public string TicketId { get; set; } = string.Empty;
        public int BookedTicketDetailsQuantity { get; set; }

        public EditBookedTicket? BookedTicket { get; set; }
        public EditTicket? Ticket { get; set; }
    }

    public class EditTicket
    {
        public string TicketId { get; set; } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public List<EditTicketDetail?> Details { get; set; } = new();
    }

    public class EditTicketResponse
    {
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public int BookedTicketDetailsQuantity { get; set; } = 0;
        public string categoryName { get; set; } = string.Empty;
    }
}
