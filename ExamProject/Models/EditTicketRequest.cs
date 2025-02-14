namespace ExamProject.Models
{
    public class EditTicketRequest
    {
        public string TicketCode { get; set; } = string.Empty;
        public int BookedTicketDetailsQuantity { get; set; }
    }
}
