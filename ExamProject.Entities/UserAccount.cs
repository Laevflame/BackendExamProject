using System;
using System.Collections.Generic;

namespace ExamProject.Entities;

public partial class UserAccount
{
    public string UserAccountId { get; set; } = null!;

    public string UserAccountName { get; set; } = null!;

    public string UserAccountEmail { get; set; } = null!;

    public string UserAccountPassword { get; set; } = null!;

    public string UserAccountAddress { get; set; } = null!;

    public string UserAccountPhoneNumber { get; set; } = null!;

    public DateTime UserAccountCreatedAt { get; set; }

    public DateTime? UserAccountModifiedAt { get; set; }

    public string? UserAccountCreatedBy { get; set; }

    public string? UserAccountModifiedBy { get; set; }

    public virtual ICollection<BookedTicket> BookedTickets { get; set; } = new List<BookedTicket>();
}
