using System;
using System.Collections.Generic;

namespace ExamProject.Entities;

public partial class BookedTicketsDetail
{
    public string BookedTicketDetailId { get; set; } = null!;

    public string BookedTicketId { get; set; } = null!;

    public string TicketId { get; set; } = null!;

    public int BookedTicketDetailsQuantity { get; set; }

    public decimal BookedTicketDetailsSubtotalPrice { get; set; }

    public string? BookedTicketDetailsSeatNumber { get; set; }

    public DateTime BookedTicketDetailsCreatedAt { get; set; }

    public DateTime? BookedTicketDetailsModifiedAt { get; set; }

    public string? BookedTicketDetailsCreatedBy { get; set; }

    public string? BookedTicketDetailsModifiedBy { get; set; }

    public virtual BookedTicket BookedTicket { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
