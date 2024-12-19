using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tiks.id_API.Models;

public partial class Theater
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Section { get; set; }

    public int Column { get; set; }

    public int Row { get; set; }
    [JsonIgnore]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
