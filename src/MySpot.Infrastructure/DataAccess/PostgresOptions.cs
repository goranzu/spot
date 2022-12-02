namespace MySpot.Infrastructure.DataAccess;

internal sealed class PostgresOptions
{
    public const string SectionName = "Postgres";
    public string ConnectionString { get; set; } 
}