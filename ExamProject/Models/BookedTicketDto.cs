namespace ExamProject.Models
{
    public class BookingResponseDto
    {
        public List<CategoryDetailsDto> Categories { get; set; } = new();
    }

    public class CategoryDetailsDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int QuantityPerCategory { get; set; }
        public List<TicketDetailsDto> Tickets { get; set; } = new();
    }

    public class TicketDetailsDto
    {
        public string TicketName { get; set; } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
    }

}
