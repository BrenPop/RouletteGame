using DerivcoAssessment;
using DerivcoAssessment.Data;
using DerivcoAssessment.Repositories;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<ISpinService, SpinService>();
builder.Services.AddScoped<ISpinRepository, SpinRepository>();
builder.Services.AddScoped<IPayoutService, PayoutService>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
