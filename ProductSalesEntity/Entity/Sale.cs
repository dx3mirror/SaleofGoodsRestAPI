using System;
using System.Collections.Generic;

namespace ProductSalesEntity.Entity;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? SaleDate { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<TrackingNumber> TrackingNumbers { get; set; } = new List<TrackingNumber>();
}
