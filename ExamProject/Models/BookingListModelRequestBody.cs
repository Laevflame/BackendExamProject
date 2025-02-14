namespace ExamProject.Models
{
    public class BookingListModelRequestBody
    {
        public string TicketCode { get; set; } = string.Empty;
        public int TicketQuantityToBook { get; set; }
    }
}
