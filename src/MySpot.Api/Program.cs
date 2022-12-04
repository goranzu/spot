using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    // register services of each layer
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddCore();
}

var app = builder.Build();

{
    // use middlewares defined by the layers
    app.UseInfrastructure();
    app.Run();
}