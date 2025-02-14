using System;
using System.Collections.Generic;

namespace ExamProject.Entities;

public partial class Ticket
{
    public string TicketId { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string TicketCode { get; set; } = null!;

    public string TicketName { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public decimal TicketPrice { get; set; }

    public int TicketQuota { get; set; }

    public int TicketRemainingQuota { get; set; }

    public bool TicketHasSeatNumber { get; set; }

    public DateTime TicketCreatedAt { get; set; }

    public DateTime? TicketModifiedAt { get; set; }

    public string? TicketCreatedBy { get; set; }

    public string? TicketModifiedBy { get; set; }

    public virtual ICollection<BookedTicketsDetail> BookedTicketsDetails { get; set; } = new List<BookedTicketsDetail>();
}
