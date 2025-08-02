using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class TicketPrice
{
    public int TicketPriceId { get; set; }

    public int TicketTypeId { get; set; }

    public int CustomerTypeId { get; set; }

    public int Price { get; set; }

    public virtual CustomerType CustomerType { get; set; } = null!;

    public virtual TicketType TicketType { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
