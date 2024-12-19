using System;
using System.Collections.Generic;

namespace Tiks.id_API.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    public virtual User User { get; set; } = null!;
}
