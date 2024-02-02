using System;
using System.Collections.Generic;

namespace ProductSalesEntity.Entity;

public partial class TrackingNumber
{
    public int TrackingNumberId { get; set; }

    public int? SaleId { get; set; }

    public string TrackingNumber1 { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual Sale? Sale { get; set; }
}
