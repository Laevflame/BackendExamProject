using System;
using System.Collections.Generic;

namespace ExamProject.Entities;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string MethodName { get; set; } = null!;

    public DateTime PaymentMethodsCreatedAt { get; set; }

    public DateTime? PaymentMethodsModifiedAt { get; set; }

    public string? PaymentMethodsCreatedBy { get; set; }

    public string? PaymentMethodsModifiedBy { get; set; }

    public virtual ICollection<BookedTicket> BookedTickets { get; set; } = new List<BookedTicket>();
}
