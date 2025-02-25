using Microsoft.EntityFrameworkCore;

namespace BmdbNetWeb.Models;

public partial class BmdbContext : DbContext {


    public BmdbContext(DbContextOptions<BmdbContext> options)
        : base(options) {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

}
