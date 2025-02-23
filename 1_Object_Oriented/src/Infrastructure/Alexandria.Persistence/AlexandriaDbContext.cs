using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence;

public class AlexandriaDbContext : DbContext
{
    public AlexandriaDbContext(DbContextOptions<AlexandriaDbContext> options)
        : base(options) { }
}
