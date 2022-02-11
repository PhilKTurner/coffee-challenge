using System.Data.Common;
using CoffeeChallenge.CoffeeStore;
using CoffeeChallenge.CoffeeStore.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStringBuilder = new DbConnectionStringBuilder();
connectionStringBuilder.ConnectionString = builder.Configuration.GetConnectionString("CoffeeStoreDatabase");

if (builder.Environment.IsDevelopment())
{
    connectionStringBuilder.Add("Password", builder.Configuration["CoffeeStoreDatabase:Password"]);
} else if (Directory.Exists("/run/secrets")) {
    builder.Configuration.AddKeyPerFile("/run/secrets");
    connectionStringBuilder.Add("Password", builder.Configuration["cc-mariadb-pw"]);
} else {
    throw new Exception("Password for database access not found.");
}

builder.Services.AddDbContext<CoffeeStoreContext>(
    options => options.UseMySql(
            connectionStringBuilder.ConnectionString,
            new MariaDbServerVersion("10.6.5"),
            options => options.EnableRetryOnFailure()
        )
);

builder.Services.AddScoped<ICoffeeStorage, CoffeeStorage>();
builder.Services.AddScoped<IStoreClerk, StoreClerk>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// In the context of this challenge Swagger can also be part of production setup.
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CoffeeStoreContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();
