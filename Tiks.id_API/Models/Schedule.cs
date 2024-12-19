using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tiks.id_API.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int TheaterId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public double Price { get; set; }
    [JsonIgnore]
    public virtual Movie Movie { get; set; } = null!;
    
    public virtual Theater Theater { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
