using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class TicketType
{
    public int TicketTypeId { get; set; }

    public string TicketTypeName { get; set; } = null!;

    public virtual ICollection<TicketPrice> TicketPrices { get; set; } = new List<TicketPrice>();
}
