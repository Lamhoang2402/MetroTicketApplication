using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class CustomerType
{
    public int CustomerTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<TicketPrice> TicketPrices { get; set; } = new List<TicketPrice>();
}
