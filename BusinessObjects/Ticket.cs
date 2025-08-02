using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Ticket
{
    public string TicketId { get; set; } = null!;

    public DateTime PurchasedAt { get; set; }

    public string? Status { get; set; }

    public int TicketPriceId { get; set; }

    public virtual TicketPrice TicketPrice { get; set; } = null!;
}
