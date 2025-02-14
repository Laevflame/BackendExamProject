using System;
using System.Collections.Generic;

namespace ExamProject.Entities;

public partial class BookedTicket
{
    public string BookedTicketId { get; set; } = null!;

    public string UserAccountId { get; set; } = null!;

    public DateTime BookedTicketDate { get; set; }

    public decimal BookedTicketTotalPrice { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal BookedTicketPaidAmount { get; set; }

    public decimal? BookedTicketChangeAmount { get; set; }

    public DateTime BookedTicketCreatedAt { get; set; }

    public DateTime? BookedTicketModifiedAt { get; set; }

    public string? BookedTicketCreatedBy { get; set; }

    public string? BookedTicketModifiedBy { get; set; }

    public virtual ICollection<BookedTicketsDetail> BookedTicketsDetails { get; set; } = new List<BookedTicketsDetail>();

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual UserAccount UserAccount { get; set; } = null!;
}
