using System;
using System.Collections.Generic;

namespace Tiks.id_API.Models;

public partial class TransactionDetail
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public string Seat { get; set; } = null!;

    public double Price { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
