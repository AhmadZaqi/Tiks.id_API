using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tiks.id_API.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }
    [JsonIgnore]
    public string Poster { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    [JsonIgnore]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
