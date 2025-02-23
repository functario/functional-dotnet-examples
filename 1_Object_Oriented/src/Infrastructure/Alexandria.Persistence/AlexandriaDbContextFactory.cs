using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Alexandria.Persistence;

public class AlexandriaDbContextFactory : IDesignTimeDbContextFactory<AlexandriaDbContext>
{
    public AlexandriaDbContext CreateDbContext(string[] args)
    {
        var sqlConnectionString = args.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(sqlConnectionString, nameof(sqlConnectionString));

        var optionsBuilder = new DbContextOptionsBuilder<AlexandriaDbContext>();
        optionsBuilder =
            (DbContextOptionsBuilder<AlexandriaDbContext>)
                ConfigureDbContextOptionsBuilder(optionsBuilder, sqlConnectionString!);

        return new AlexandriaDbContext(optionsBuilder.Options);
    }

    public void MigrateDb(string sqlConnectionString)
    {
        using var context = CreateDbContext([sqlConnectionString]);
        context.Database.Migrate();
    }

    public static DbContextOptionsBuilder ConfigureDbContextOptionsBuilder(
        DbContextOptionsBuilder optionsBuilder,
        string sqlConnectionString
    )
    {
        optionsBuilder.UseSqlServer(
            sqlConnectionString,
            x => { }
        //x.MigrationsHistoryTable(
        //    HistoryRepository.DefaultTableName,
        //    PersistenceConstants.DefaultSchema
        //)
        );

        return optionsBuilder;
    }
}
