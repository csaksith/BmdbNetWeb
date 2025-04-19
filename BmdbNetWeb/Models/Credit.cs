using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BmdbNetWeb.Models;

[Table("Credit")]
[Index("MovieId", "ActorId", Name = "UC_Credit", IsUnique = true)]
public partial class Credit
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MovieID")]
    public int MovieId { get; set; }

    [Column("ActorID")]
    public int ActorId { get; set; }

    public string Role { get; set; } = null!;

    public Actor? Actor { get; set; } = null!;

    public Movie? Movie { get; set; } = null!;
}
