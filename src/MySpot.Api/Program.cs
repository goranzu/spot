using MySpot.Application;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services
        .AddInfrastructure()
        .AddApplication();
}

var app = builder.Build();

{
    app.MapControllers();
    app.Run();
}